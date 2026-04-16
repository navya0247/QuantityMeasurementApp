using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;

var logger = LogManager.Setup()
    .LoadConfiguration(builder =>
    {
        builder.ForLogger()
               .FilterMinLevel(NLog.LogLevel.Info)
               .WriteToConsole(layout: "${longdate} | ${level:uppercase=true} | [API-GATEWAY] | ${message}");
    })
    .GetCurrentClassLogger();

logger.Info("API Gateway starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var jwtKey = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt:Key missing from appsettings.json");

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

    builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    var app = builder.Build();

    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    // Root - redirect to swagger UI
    app.MapGet("/", () => Results.Redirect("/swagger"));

    // Combined Swagger UI — fetches swagger.json via gateway proxy routes (no CORS issues)
    app.MapGet("/swagger", () => Results.Content("""
<!DOCTYPE html>
<html>
<head>
    <title>API Gateway - All Services</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" type="text/css" href="https://unpkg.com/swagger-ui-dist@5/swagger-ui.css">
    <style>
        body { margin: 0; background: #fafafa; }
        .topbar { background: #1b1b1b !important; }
        .topbar-wrapper img { display: none; }
        .topbar-wrapper::after { content: 'QMA API Gateway — All Services'; color: white; font-size: 1.2rem; font-weight: bold; padding-left: 16px; }
        .scheme-container { background: #fff; padding: 16px 0; }
    </style>
</head>
<body>
<div id="swagger-ui"></div>
<script src="https://unpkg.com/swagger-ui-dist@5/swagger-ui-bundle.js"></script>
<script src="https://unpkg.com/swagger-ui-dist@5/swagger-ui-standalone-preset.js"></script>
<script>
window.onload = function() {
    SwaggerUIBundle({
        urls: [
            { url: "/auth-docs/swagger/v1/swagger.json", name: "Auth Service - signup · signin · profile" },
            { url: "/qma-docs/swagger/v1/swagger.json",  name: "QMA Service - compare · convert · add · subtract · divide · history" }
        ],
        "urls.primaryName": "Auth Service - signup · signin · profile",
        dom_id: '#swagger-ui',
        deepLinking: true,
        presets: [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset],
        plugins: [SwaggerUIBundle.plugins.DownloadUrl],
        layout: "StandaloneLayout"
    });
};
</script>
</body>
</html>
""", "text/html"));

    app.MapReverseProxy();

    logger.Info("API Gateway started on port 5003.");
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "API Gateway failed to start.");
    throw;
}
finally
{
    LogManager.Shutdown();
}
