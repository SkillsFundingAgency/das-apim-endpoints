using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        //public static string WriteErrorResponse(DomainException domainException)
        //{
        //    var response = new ErrorResponse(MapToApiErrors(domainException.DomainErrors));
        //    return JsonConvert.SerializeObject(response);
        //}

        //public static string WriteErrorResponse(BulkUploadDomainException domainException)
        //{
        //    var response = new BulkUploadErrorResponse(domainException.DomainErrors);
        //    var responseAsString = JsonConvert.SerializeObject(response);
        //    return responseAsString;
        //}

        //private static List<ErrorDetail> MapToApiErrors(IEnumerable<DomainError> source)
        //{
        //    return source.Select(sourceItem => new ErrorDetail(sourceItem.PropertyName, sourceItem.ErrorMessage)).ToList();
        //}
    }
}
