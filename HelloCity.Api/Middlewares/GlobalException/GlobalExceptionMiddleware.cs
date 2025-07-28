using System.Text.Json;
using Serilog;

namespace HelloCity.Api.Middlewares.GlobalException
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception ex) 
            {
                _logger.LogError(ex, "Unhandled exception occured");

                int statusCode = ExceptionHandler.GetStatusCode(ex);
                string message = ExceptionHandler.GetErrorMessage(ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var errorResponse = new
                {
                    status = statusCode,
                    message,
                    // Optionally include more details in development mode; Details should not be exposed in production
                    detials = ex.Message
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
            }
        }
    }
}
