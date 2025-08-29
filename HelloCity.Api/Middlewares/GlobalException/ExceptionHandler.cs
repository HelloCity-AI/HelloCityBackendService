using System.Reflection.Metadata.Ecma335;

namespace HelloCity.Api.Middlewares.GlobalException
{
    public static class ExceptionHandler
    {
        public static int GetStatusCode(Exception ex)
        {
            return ex switch
            {
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                BadHttpRequestException => StatusCodes.Status413PayloadTooLarge,
                NotSupportedException => StatusCodes.Status415UnsupportedMediaType,
                System.Security.SecurityException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        public static string GetErrorMessage(Exception ex)
        {
            return ex switch
            {
                ArgumentNullException => "Required value is missing.",
                ArgumentException => "An invalid argument was provided,",
                KeyNotFoundException => "The requested resource was not found.",
                UnauthorizedAccessException => "Access is denied due to invalid credentials.",
                BadHttpRequestException => "The request is invalid: file size exceeds 5 MB limit",
                NotSupportedException => "Unsupported media type.",
                System.Security.SecurityException => "You do not have permission to perform this action.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }
    }
}
