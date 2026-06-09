using System.Net;
using System.Text.Json;
using Versus.API.DTOs.Responses;

namespace Versus.API.Middleware
{
    // Custom exceptions
    public class NotFoundException(string message) : Exception(message);
    public class ForbiddenAccessException(string message) : Exception(message);
    public class ConflictException(string message) : Exception(message);
    public class BadGatewayException(string message) : Exception(message);

    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (code, message) = ex switch
            {
                ArgumentException e => (HttpStatusCode.BadRequest, e.Message),
                UnauthorizedAccessException e => (HttpStatusCode.Unauthorized, e.Message),
                ForbiddenAccessException e => (HttpStatusCode.Forbidden, e.Message),
                NotFoundException e => (HttpStatusCode.NotFound, e.Message),
                ConflictException e => (HttpStatusCode.Conflict, e.Message),
                BadGatewayException e => (HttpStatusCode.BadGateway, e.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            logger.LogError(ex, "HTTP {StatusCode} — {Message}", (int)code, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var response = new ApiResponse<object> { Success = false, Message = message };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}