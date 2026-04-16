using Application.Core;
using FluentValidation;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment env) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericException(context, ex, logger, env);
            }
        }

        private static async Task HandleValidationException(HttpContext context, ValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errorMessage = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));

            var result = Result<object>.Failure(errorMessage, StatusCodes.Status400BadRequest);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
        }

        private static async Task HandleGenericException(HttpContext context, Exception ex, ILogger logger, IHostEnvironment env)
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var message = env.IsDevelopment()
                ? $"{ex.Message} | StackTrace: {ex.StackTrace}"
                : "Internal Server Error";

            var result = Result<object>.Failure(message, StatusCodes.Status500InternalServerError);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
        }
    }
}