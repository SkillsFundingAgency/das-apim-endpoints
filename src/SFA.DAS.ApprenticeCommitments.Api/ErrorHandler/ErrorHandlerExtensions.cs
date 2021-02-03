using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.ApprenticeCommitments.Api.ErrorHandler
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
                    if (contextFeature.Error is HttpRequestContentException httpException)
                    {
                        logger.LogError(httpException, "HttpRequestContentException");

                        context.Response.StatusCode = (int) httpException.StatusCode;
                        if (!string.IsNullOrWhiteSpace(httpException.ErrorContent))
                        {
                            logger.LogError($"Inner Api returned error content: {httpException.ErrorContent}");
                            await context.Response.WriteAsync(httpException.ErrorContent);
                        }
                    }
                    else if (contextFeature.Error is Exception exception)
                    {
                        logger.LogError(exception, "Unhandled Exception");
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