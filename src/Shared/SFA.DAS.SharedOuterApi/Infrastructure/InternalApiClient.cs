using System.Linq;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.Api.Common.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class InternalApiClient<T> : ApiClient<T>, IInternalApiClient<T> where T : IInternalApiConfiguration
    {
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InternalApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IAzureClientCredentialHelper azureClientCredentialHelper,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, apiConfiguration)
        {
            _azureClientCredentialHelper = azureClientCredentialHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            if (!string.IsNullOrEmpty(Configuration.Identifier))
            {
                var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(Configuration.Identifier);
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var currentAuthHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (currentAuthHeader != null)
            {
                httpRequestMessage.Headers.Add("Authorization", currentAuthHeader);
            }
        }
    }
}
