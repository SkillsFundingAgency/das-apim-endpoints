using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.Api.Common.Interfaces;
using Microsoft.Azure.Services.AppAuthentication;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class InternalApiClient<T> : ApiClient<T>, IInternalApiClient<T> where T : IInternalApiConfiguration
    {
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;

        public InternalApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IAzureClientCredentialHelper azureClientCredentialHelper) : base(httpClientFactory, apiConfiguration)
        {
            _azureClientCredentialHelper = azureClientCredentialHelper;
        }

        protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            if (!string.IsNullOrEmpty(Configuration.Identifier))
            {
                //var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(Configuration.Identifier);
                
                // TEMP THIS WILL NOT BE MERGED
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(Configuration.Identifier, true);

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);               
            }
        }
    }
}
