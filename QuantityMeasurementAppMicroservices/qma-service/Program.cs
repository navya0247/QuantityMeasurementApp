using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using QmaService.Data;
using QmaService.Interfaces;
using QmaService.Repositories;
using QmaService.Services;
using StackExchange.Redis;

var logger = LogManager.Setup()
    .LoadConfiguration(builder =>
    {
        builder.ForLogger()
               .FilterMinLevel(NLog.LogLevel.Info)
               .WriteToConsole(layout: "${longdate} | ${level:uppercase=true} | [QMA-SERVICE] | ${message}");

        builder.ForLogger()
               .FilterMinLevel(NLog.LogLevel.Info)
               .WriteToFile(
                   fileName: "${basedir}/logs/qma-${shortdate}.log",
                   layout: "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=tostring}");
    })
    .GetCurrentClassLogger();

logger.Info("QMA Service starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // ── EF Core (PostgreSQL / InMemory fallback) ──
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<QmaDbContext>(options =>
    {
        if (!string.IsNullOrEmpty(connStr))
            options.UseNpgsql(connStr);
        else
            options.UseInMemoryDatabase("QmaDB");
    });

    // ── Redis — falls back to EF Core repository if Redis not running ──
    var redisConnStr = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    bool redisAvailable = false;
    try
    {
        var redis = ConnectionMultiplexer.Connect(redisConnStr);
        builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
        builder.Services.AddScoped<IQuantityMeasurementRepository, RedisQuantityRepository>();
        redisAvailable = true;
        logger.Info("Redis connected — using Redis repository.");
    }
    catch
    {
        builder.Services.AddScoped<IQuantityMeasurementRepository, EFQuantityRepository>();
        logger.Warn("Redis unavailable — falling back to EF Core repository.");
    }

    // ── Business Service ──
    builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

    // ── JWT Auth (validates tokens issued by auth-service) ──
    var jwtKey = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt:Key is missing from appsettings.json");

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = builder.Configuration["Jwt:Issuer"] ?? "QuantityMeasurementApp",
                ValidAudience            = builder.Configuration["Jwt:Audience"] ?? "QuantityMeasurementApp",
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

    builder.Services.AddAuthorization();

    // ── Controllers + Swagger ──
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title   = "QMA Service API",
            Version = "v1",
            Description = "Handles quantity measurements and history. Redis cache layer included."
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name        = "Authorization",
            Type        = SecuritySchemeType.Http,
            Scheme      = "Bearer",
            BearerFormat = "JWT",
            In          = ParameterLocation.Header,
            Description = "Paste your JWT token here."
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id   = "Bearer"
                    }
                },
                new List<string>()
            }
        });
    });

    // ── CORS ──
    builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

    var app = builder.Build();

    // ── Migrations ──
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var db  = scope.ServiceProvider.GetRequiredService<QmaDbContext>();
            var log = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            if (db.Database.IsRelational())
                db.Database.Migrate();
            else
                db.Database.EnsureCreated();
            log.LogInformation("QMA migrations applied. Redis available: {Redis}", redisAvailable);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "QMA Service migration failed.");
        }
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QMA Service v1");
        c.RoutePrefix = "swagger";
    });

    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    // ── Root → Swagger redirect ──
    app.MapGet("/", () => Results.Redirect("/swagger"));
    app.MapGet("/health", () => Results.Ok(new
    {
        service = "qma-service",
        status  = "healthy",
        redis   = redisAvailable
    }));

    app.MapControllers();

    logger.Info("QMA Service started on port 5002.");
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "QMA Service failed to start.");
    throw;
}
finally
{
    LogManager.Shutdown();
}