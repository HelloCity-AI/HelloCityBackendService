using System.CodeDom.Compiler;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HelloCity.Api.HealthChecks;

public static class HealthCheckResponseWriter
{
    public static Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        context.Response.StatusCode = healthReport.Status == HealthStatus.Healthy
            ? StatusCodes.Status200OK
            : StatusCodes.Status503ServiceUnavailable;

        var result = new
        {
            status = healthReport.Status.ToString(),
            results = healthReport.Entries.ToDictionary(e => e.Key, e => new
            {
                status = context.Response.StatusCode,
                description = e.Value.Description
            })
        };

        var jsonResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return context.Response.WriteAsync(jsonResult);
    }

}