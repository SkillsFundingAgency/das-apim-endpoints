using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.Api.Common.Interfaces;
using System.Text;
using System;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class LepsLaExternalApiClient<T> : ApiClient<T>, ILepsLaExternalApiClient<T> where T : ILepsLaExternalApiConfiguration
    {
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;

        public LepsLaExternalApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IAzureClientCredentialHelper azureClientCredentialHelper) : base(httpClientFactory, apiConfiguration)
        {
            _azureClientCredentialHelper = azureClientCredentialHelper;
        }

        protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            var username = Configuration.ClientId;
            var password = Configuration.ClientSecret;
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }
    }
}
