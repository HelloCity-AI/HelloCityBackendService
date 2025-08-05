using System.Text.Json;
using Serilog;

namespace HelloCity.Api.Middlewares.GlobalException
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;


        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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
                    details = _env.IsDevelopment() ? (ex.InnerException?.Message ?? ex.Message) : null
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
            }
        }
    }
}
