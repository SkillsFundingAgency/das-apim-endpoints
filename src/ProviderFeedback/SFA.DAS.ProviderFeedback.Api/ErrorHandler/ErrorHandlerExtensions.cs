﻿using Microsoft.AspNetCore.Diagnostics;

namespace SFA.DAS.ProviderFeedback.Api.ErrorHandler
{
    public static class ErrorHandlerExtensions
    {
        public static IApplicationBuilder UseApiGlobalExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
#pragma warning disable 1998
            async Task Handler(HttpContext context)
#pragma warning restore 1998
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    if (contextFeature.Error is Exception exception)
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