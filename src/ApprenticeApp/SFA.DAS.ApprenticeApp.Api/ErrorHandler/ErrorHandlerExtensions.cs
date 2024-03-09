using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Api.ErrorHandler
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

                        context.Response.StatusCode = (int)httpException.StatusCode;
                        if (!string.IsNullOrWhiteSpace(httpException.ErrorContent))
                        {
                            logger.LogError($"Inner Api returned error content: {httpException.ErrorContent}");
                            await context.Response.WriteAsync(httpException.ErrorContent);
                        }
                    }
                    else if (contextFeature.Error is ValidationException ex)
                    {
                        logger.LogError(ex, "ValidationException");
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(CreateErrorResponse(ex.Errors));
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
        private static string CreateErrorResponse(IEnumerable<ValidationFailure> errors)
        {
            var errorList = errors.Select(x => new ErrorItem { PropertyName = x.PropertyName, ErrorMessage = x.ErrorMessage });
            return JsonConvert.SerializeObject(errorList, Formatting.Indented);
        }

    }

    public class ErrorItem
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}