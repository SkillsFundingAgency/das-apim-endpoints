using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApprenticeCommitments.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public sealed class GenericInnerApiQueryClient
    {
        private readonly ApprenticeCommitmentsConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
        private readonly IHttpClientFactory _clientFactory;

        public GenericInnerApiQueryClient(
            ApprenticeCommitmentsConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            IAzureClientCredentialHelper azureClientCredentialHelper,
            IHttpClientFactory clientFactory)
        {
            _azureClientCredentialHelper = azureClientCredentialHelper;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            var client = await CreateClient();
            var response = await client.GetAsync(path);
            return response;
        }

        public async Task<HttpClient> CreateClient()
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.Url);

            await AddAuthenticationHeader(client);
            AddVersionHeader("1.0", client);

            return client;
        }

        private async Task AddAuthenticationHeader(HttpClient client)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_configuration.Identifier);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        private void AddVersionHeader(string requestVersion, HttpClient client)
        {
            client.DefaultRequestHeaders.Remove("X-Version");
            client.DefaultRequestHeaders.Add("X-Version", requestVersion);
        }
    }
}