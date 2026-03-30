using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using QuantityMeasurementApp.API.Middleware;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.RepoLayer.Data;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Repositories;
using StackExchange.Redis;

var logger = LogManager.Setup()
    .LoadConfiguration(builder =>
    {
        builder.ForLogger()
               .FilterMinLevel(NLog.LogLevel.Info)
               .WriteToFile(
                   fileName: "${basedir}/logs/app-${shortdate}.log",
                   layout: "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=tostring}");

        builder.ForLogger()
               .FilterMinLevel(NLog.LogLevel.Error)
               .WriteToFile(
                   fileName: "${basedir}/logs/errors-${shortdate}.log",
                   layout: "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=tostring}");

        builder.ForLogger()
               .FilterMinLevel(NLog.LogLevel.Info)
               .WriteToConsole(layout: "${longdate} | ${level:uppercase=true} | ${message}");
    })
    .GetCurrentClassLogger();

logger.Info("Application starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //  NLog replaces all default logging providers 
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Controllers
    builder.Services.AddControllers();

    //  EF Core — SQL Server (falls back to in-memory if connection fails) 
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        try { options.UseSqlServer(connectionString); }
        catch { options.UseInMemoryDatabase("QuantityMeasurementDB"); }
    });

    //  Redis - falls back to SQL Server repository if Redis not running 
    var redisConnStr = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    bool redisAvailable = false;
    try
    {
        var redis = ConnectionMultiplexer.Connect(redisConnStr);
        builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
        builder.Services.AddScoped<IQuantityMeasurementRepository, RedisQuantityRepository>();
        redisAvailable = true;
    }
    catch
    {
        // Redis not available — use SQL Server repository instead
        builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementRepository>();
    }

    //  Repositories 
    builder.Services.AddScoped<IUserRepository, EFUserRepository>();

    //  Business Services 
    builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

    // - Security Services 
    // EncryptionService: AES-256 — registered for future use 
    builder.Services.AddScoped<IEncryptionService, EncryptionService>();

    // - JWT Authentication 
    // Reads key/issuer/audience directly from config — avoids the anti-pattern
    // of calling BuildServiceProvider() during service registration, which
    // creates a second DI container and can cause scoped-service bugs.
    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtKey = builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is missing from appsettings.json");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "QuantityMeasurementApp",
                ValidAudience = builder.Configuration["Jwt:Audience"] ?? "QuantityMeasurementApp",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

    builder.Services.AddAuthorization();

    //- Swagger / OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Quantity Measurement API",
            Version = "v1",
            Description = "ASP.NET Core REST API with JWT authentication, Redis caching, EF Core persistence."
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);

        // Swagger UI - paste JWT token in the Authorize button (no Bearer prefix needed)
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Paste your JWT token here. No need to add Bearer prefix."
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

    // - CORS — allow all origins for development
    builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

    // - Build 
    var app = builder.Build();

    // - Apply EF Core Migrations on startup 
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var nlog = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            db.Database.Migrate();
            nlog.LogInformation("Migrations applied. Redis available: {Redis}", redisAvailable);
        }
        catch (System.Exception ex)
        {
            logger.Error(ex, "Migration failed — check SQL Server connection string.");
        }
    }

    // - Middleware 
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
            c.RoutePrefix = string.Empty; // Swagger at root URL
        });
    }

    app.UseHttpsRedirection();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    logger.Info("Application started successfully.");
    app.Run();
}
catch (System.Exception ex)
{
    logger.Error(ex, "Application failed to start.");
    throw;
}
finally
{
    LogManager.Shutdown();
}

public partial class Program { }