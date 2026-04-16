using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using AuthService.Data;
using AuthService.Interfaces;
using AuthService.Repositories;
using AuthService.Services;

var logger = LogManager.Setup()
    .LoadConfiguration(builder =>
    {
        builder.ForLogger()
               .FilterMinLevel(NLog.LogLevel.Info)
               .WriteToConsole(layout: "${longdate} | ${level:uppercase=true} | [AUTH-SERVICE] | ${message}");

        builder.ForLogger()
               .FilterMinLevel(NLog.LogLevel.Info)
               .WriteToFile(
                   fileName: "${basedir}/logs/auth-${shortdate}.log",
                   layout: "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=tostring}");
    })
    .GetCurrentClassLogger();

logger.Info("Auth Service starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    //  EF Core (PostgreSQL / InMemory fallback) 
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<AuthDbContext>(options =>
    {
        if (!string.IsNullOrEmpty(connStr))
            options.UseNpgsql(connStr);
        else
            options.UseInMemoryDatabase("AuthDB");
    });

    // ── Repositories ──
    builder.Services.AddScoped<IUserRepository, EFUserRepository>();

    // ── Services ──
    builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
    builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
    builder.Services.AddScoped<IEncryptionService, EncryptionService>();

    // ── JWT Auth ──
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
            Title   = "Auth Service API",
            Version = "v1",
            Description = "Handles user registration, login and JWT token management."
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
            var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            if (db.Database.IsRelational())
                db.Database.Migrate();
            else
                db.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Auth Service migration failed.");
        }
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service v1");
        c.RoutePrefix = "swagger";
    });

    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    // ── Root → Swagger redirect ──
    app.MapGet("/", () => Results.Redirect("/swagger"));
    app.MapGet("/health", () => Results.Ok(new { service = "auth-service", status = "healthy" }));
    app.MapControllers();

    logger.Info("Auth Service started on port 5001.");
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Auth Service failed to start.");
    throw;
}
finally
{
    LogManager.Shutdown();
}