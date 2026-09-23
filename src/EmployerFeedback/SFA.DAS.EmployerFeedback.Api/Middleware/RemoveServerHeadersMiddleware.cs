using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.EmployerFeedback.Api.Middleware
{
    public class RemoveServerHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public RemoveServerHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("Server");

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
