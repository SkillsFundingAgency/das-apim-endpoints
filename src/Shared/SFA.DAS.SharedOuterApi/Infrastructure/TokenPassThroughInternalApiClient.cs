using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.SharedOuterApi.Interfaces;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class TokenPassThroughInternalApiClient<T> : ApiClient<T>, ITokenPassThroughInternalApiClient<T> where T : IInternalApiConfiguration
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TokenPassThroughInternalApiClient<T>> _logger;

        /// <summary>
        /// ApiClient used for requests to APIs that require tokens with account-level claims for Authorization.
        /// </summary>
        public TokenPassThroughInternalApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TokenPassThroughInternalApiClient<T>> logger) : base(httpClientFactory, apiConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Stores the bearer token from the incoming request of the current <see cref="HttpContext"/> to the Authorization header of the outgoing request
        /// </summary>
        protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-Authorization"].FirstOrDefault();
            if (authHeader != null)
            {
                httpRequestMessage.Headers.Add("Authorization", authHeader);
            }
            else
            {
                _logger.LogWarning("Bearer token not received in header 'X-Forwarded-Authorization', and therefore no Authorization header was attached to request message.");
            }

            return Task.CompletedTask;
        }
    }
}