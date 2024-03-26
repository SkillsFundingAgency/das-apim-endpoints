using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class TokenPassThroughInternalApiClient<T> : ApiClient<T>, ITokenPassThroughInternalApiClient<T> where T : IInternalApiConfiguration
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenPassThroughInternalApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, apiConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null)
            {
                httpRequestMessage.Headers.Add("Authorization", authHeader);
            }

            return Task.CompletedTask;
        }
    }
}