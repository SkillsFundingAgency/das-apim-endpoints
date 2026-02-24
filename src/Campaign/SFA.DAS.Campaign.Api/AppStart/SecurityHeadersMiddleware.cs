using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Api.AppStart;

[ExcludeFromCodeCoverage]
public class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.AddIfNotPresent("x-frame-options", new StringValues("DENY"));
        context.Response.Headers.AddIfNotPresent("x-content-type-options", new StringValues("nosniff"));
        context.Response.Headers.AddIfNotPresent("X-Permitted-Cross-Domain-Policies", new StringValues("none"));
        context.Response.Headers.AddIfNotPresent("x-xss-protection", new StringValues("0"));
        context.Response.Headers.AddIfNotPresent("Content-Security-Policy", new StringValues($"default-src *; script-src *; connect-src *; img-src *; style-src *; object-src *;"));
        context.Response.Headers.AddIfNotPresent("Strict-Transport-Security", new StringValues("max-age=31536000"));

        await next(context);
    }
}
