using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using QuantityMeasurementApp.API.Middleware;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.BusinessLayer.Services;
using QuantityMeasurementApp.RepoLayer.Data;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Repositories;
using StackExchange.Redis;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ─ Controllers
builder.Services.AddControllers();

// ─ EF Core — SQL Server (falls back to InMemory if SQL Server not available) ──
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    try
    {
        options.UseSqlServer(connectionString);
    }
    catch
    {
        options.UseInMemoryDatabase("QuantityMeasurementDB");
    }
});

// ── Redis — tries to connect, falls back to InMemory repo if Redis not running ──
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
    // Redis not running — fall back to EF Core SQL Server repository
    builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementRepository>();
}

//  User Repository 
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

// Business Services
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

//  JWT Authentication 
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtService = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<IJwtTokenService>();
        options.TokenValidationParameters = jwtService.GetValidationParameters();
    });

builder.Services.AddAuthorization();

// Swagger / OpenAPI 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quantity Measurement API",
        Version = "v1",
        Description = "UC17 — ASP.NET Core REST API with JWT authentication."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);

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

//  CORS 
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

//  Build 
var app = builder.Build();

//  Create DB tables automatically using EF Core 
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate(); // applies pending migrations automatically 
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Database ready. Redis available: {RedisAvailable}", redisAvailable);
    }
    catch (System.Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning("DB setup failed — using InMemory. Error: {Message}", ex.Message);
    }
}

//  Middleware pipeline 
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
