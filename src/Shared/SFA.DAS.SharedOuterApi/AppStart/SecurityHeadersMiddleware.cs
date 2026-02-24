using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.AppStart
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate next;
        public SecurityHeadersMiddleware(RequestDelegate next) => this.next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Remove("Server");
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("X-AspNet-Version");
                context.Response.Headers.Remove("X-AspNetMvc-Version");

                return Task.CompletedTask;
            });

            context.Response.Headers.AddIfNotPresent("x-frame-options", new StringValues("DENY"));
            context.Response.Headers.AddIfNotPresent("x-content-type-options", new StringValues("nosniff"));
            context.Response.Headers.AddIfNotPresent("X-Permitted-Cross-Domain-Policies", new StringValues("none"));
            context.Response.Headers.AddIfNotPresent("x-xss-protection", new StringValues("0"));
            context.Response.Headers.AddIfNotPresent("Content-Security-Policy", new StringValues($"default-src *; script-src *; connect-src *; img-src *; style-src *; object-src *;"));

            await next(context);
        }
    }
}
