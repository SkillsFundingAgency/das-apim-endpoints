using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class LoggingApiHandler<T> : DelegatingHandler 
    {
        private readonly ILogger<T> _logger;

        public LoggingApiHandler(ILogger<T> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Calling endpoint {request?.RequestUri?.AbsoluteUri}");
                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error when calling endpoint {request?.RequestUri?.AbsoluteUri}", e);
                throw;
            }
        }
    }
}
