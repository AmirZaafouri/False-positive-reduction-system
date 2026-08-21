using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware
{
    /// <summary>
    /// Catches all unhandled exceptions and returns a structured RFC 7807
    /// ProblemDetails response instead of a raw 500 stack trace.
    /// </summary>
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "Unhandled exception on {Method} {Path}",
                httpContext.Request.Method,
                httpContext.Request.Path);

            var (statusCode, title) = exception switch
            {
                ArgumentException or FormatException
                    => (StatusCodes.Status400BadRequest, "Bad Request"),
                System.Text.Json.JsonException
                    => (StatusCodes.Status400BadRequest, "Invalid JSON payload"),
                _
                    => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}
