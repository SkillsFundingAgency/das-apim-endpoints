using System.Net.Http.Headers;
using SFA.DAS.Api.Common.Interfaces;
using Newtonsoft.Json.Linq;
using SFA.DAS.EarlyConnect.Services.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EarlyConnect.Services.LepsApiClients
{
    public class LepsNeApiClient<T> : ApiClient<T>, ILepsNeApiClient<T> where T : ILepsNeApiConfiguration
    {
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;

        public LepsNeApiClient(
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
                var accessToken = await GetAccessTokenAsync();
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
        private async Task<string> GetAccessTokenAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "client_id", Configuration.ClientId },
                    { "client_secret", Configuration.ClientSecret },
                    { "scope", Configuration.Scope },
                    { "grant_type", "client_credentials" }
                };

                var content = new FormUrlEncodedContent(parameters);

                var response = await httpClient.PostAsync(Configuration.Identifier, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(responseContent);
                var accessToken = json["access_token"].ToString();
                return accessToken;
            }
        }
    }
}
