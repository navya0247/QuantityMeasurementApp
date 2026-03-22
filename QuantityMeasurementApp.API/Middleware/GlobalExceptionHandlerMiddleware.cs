using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SystemException = System.Exception;

namespace QuantityMeasurementApp.API.Middleware
{
    /// <summary>Catches all unhandled exceptions and returns a consistent JSON error response.</summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try { await _next(context); }
            catch (SystemException ex)
            {
                _logger.LogError(ex, "Unhandled exception at {Path}", context.Request.Path);
                await HandleAsync(context, ex);
            }
        }

        private static Task HandleAsync(HttpContext context, SystemException ex)
        {
            var (status, error) = ex switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized,        "Unauthorised"),
                InvalidOperationException   => (HttpStatusCode.BadRequest,          "Bad Request"),
                KeyNotFoundException        => (HttpStatusCode.NotFound,            "Not Found"),
                ArgumentException           => (HttpStatusCode.BadRequest,          "Bad Request"),
                NotSupportedException       => (HttpStatusCode.BadRequest,          "Unsupported Operation"),
                ArithmeticException         => (HttpStatusCode.BadRequest,          "Arithmetic Error"),
                _                           => (HttpStatusCode.InternalServerError, "Internal Server Error")
            };

            string body = JsonSerializer.Serialize(new
            {
                timestamp = DateTime.UtcNow.ToString("o"),
                status    = (int)status,
                error,
                message   = ex.Message,
                path      = context.Request.Path.Value
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode  = (int)status;
            return context.Response.WriteAsync(body);
        }
    }
}
