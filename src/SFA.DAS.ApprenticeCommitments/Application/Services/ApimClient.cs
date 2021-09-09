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
    public class ApimClient
    {
        private readonly HttpClient _httpClient;
        private readonly Func<Task> _addAuthentication;

        public ApimClient(
            IHttpClientFactory httpClientFactory,
            IOwnerApiConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            IAzureClientCredentialHelper azureClientCredentialHelper)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration.Url);

            if (hostingEnvironment.IsDevelopment())
                _addAuthentication = () => Task.CompletedTask;
            else
                _addAuthentication = async () => await AddAuthentication();

            async Task AddAuthentication()
            {
                var accessToken = await azureClientCredentialHelper.GetAccessTokenAsync(configuration.Identifier);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public async Task<HttpClient> PrepareClient(string apiVersion = "1.0")
        {
            await _addAuthentication();
            AddVersionHeader(apiVersion);
            return _httpClient;
        }

        private void AddVersionHeader(string requestVersion)
        {
            _httpClient.DefaultRequestHeaders.Remove("X-Version");
            _httpClient.DefaultRequestHeaders.Add("X-Version", requestVersion);
        }
    }
}