using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class TokenPassThroughInternalApiClient<T> : ApiClient<T>, ITokenPassThroughInternalApiClient<T> where T : IInternalApiConfiguration
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TokenPassThroughInternalApiClient<T>> _logger;

        public TokenPassThroughInternalApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TokenPassThroughInternalApiClient<T>> logger) : base(httpClientFactory, apiConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-Authorization"].FirstOrDefault();
            if (authHeader != null)
            {
                httpRequestMessage.Headers.Add("Authorization", authHeader);
                _logger.LogInformation($"Added bearer token from incoming X-Forwarded-Authorization header {authHeader} to outgoing HttpRequestMessage");
            }
            else
            {
                _logger.LogInformation($"Bearer Token not received, no token sent");
            }

            return Task.CompletedTask;
        }

        protected override void LogMessage(string message)
        {
            _logger.LogInformation(message);
        }
    }
}