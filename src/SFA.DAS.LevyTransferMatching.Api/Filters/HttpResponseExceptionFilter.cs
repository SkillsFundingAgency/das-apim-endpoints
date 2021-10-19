using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.LevyTransferMatching.Api.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger<HttpResponseExceptionFilter> _logger;

        public HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("There has been an unhandled error that has bubbled up the stack");

            if (context.Exception is HttpResponseException exception)
            {
                _logger.LogInformation($"The error has occurred internally : \n\n{exception}");
                
                context.Result = new ObjectResult("An internal error occurred")
                {
                    StatusCode = exception.Status
                };
            }
            else
            {
                _logger.LogInformation($"The error has occurred externally : \n\n{context.Exception}");

                context.Result = new ObjectResult("An external error occurred")
                {
                    StatusCode = 500
                };
            }

            context.ExceptionHandled = true;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public int Order { get; } = int.MaxValue - 10; //run after every other filter in the pipeline
    }

    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 400;

        public object Value { get; set; }
    }
}
