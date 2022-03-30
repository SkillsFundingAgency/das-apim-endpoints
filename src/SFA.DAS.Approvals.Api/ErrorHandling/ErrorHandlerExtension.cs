using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Approvals.ErrorHandling
{
    public static class ErrorHandlerExtensions
    {
        public static IApplicationBuilder UseApiGlobalExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            async Task Handler(HttpContext context)
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    if (contextFeature.Error is DomainApimException modelException)
                    {
                        context.Response.SetStatusCode(HttpStatusCode.BadRequest);
                        context.Response.SetSubStatusCode(HttpSubStatusCode.DomainException);
                        logger.LogError($"Model Error thrown: {modelException}");
                        await context.Response.WriteAsync(modelException.Content);
                    }
                    if (contextFeature.Error is BulkUploadApimDomainException bulkUploadDomainException)
                    {
                        context.Response.SetStatusCode(HttpStatusCode.BadRequest);
                        context.Response.SetSubStatusCode(HttpSubStatusCode.BulkUploadDomainException);
                        logger.LogError($"Model Error thrown: {bulkUploadDomainException}");
                        await context.Response.WriteAsync(bulkUploadDomainException.Content);
                    }
                    else
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                    }
                }
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(Handler);
            });
            return app;
        }
    }
}
