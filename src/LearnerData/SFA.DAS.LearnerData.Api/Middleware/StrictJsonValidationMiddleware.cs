using System.Text;
using Newtonsoft.Json;

namespace SFA.DAS.LearnerData.Api.Middleware;

public class StrictJsonValidationMiddleware<T>(RequestDelegate next, ILogger<StrictJsonValidationMiddleware<T>> logger)
    where T : class
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/Stub/providers", StringComparison.OrdinalIgnoreCase) &&
            context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
        {
            context.Request.EnableBuffering();
            context.Request.Body.Position = 0;

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var rawJson = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error
            };

            try
            {
                var result = JsonConvert.DeserializeObject<T>(rawJson, settings);
            }
            catch (JsonSerializationException ex)
            {
                logger.LogError("Strict deserialization failed: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Unexpected or invalid fields in payload",
                    details = ex.Message
                });
                return;
            }
            catch (JsonReaderException ex)
            {
                logger.LogError("Strict deserialization failed: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Invalid JSON format in payload",
                    details = ex.Message
                });
                return;
            }
        }

        await next(context);
    }
}