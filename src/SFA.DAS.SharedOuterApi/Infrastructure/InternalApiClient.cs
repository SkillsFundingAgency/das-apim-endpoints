﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.Api.Common.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class InternalApiClient<T> : ApiClient<T>, IInternalApiClient<T> where T : IInternalApiConfiguration
    {
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;

        public InternalApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IWebHostEnvironment hostingEnvironment,
            IAzureClientCredentialHelper azureClientCredentialHelper) : base(httpClientFactory, apiConfiguration, hostingEnvironment)
        {
            _azureClientCredentialHelper = azureClientCredentialHelper;
        }

        protected override async Task AddAuthenticationHeader(HttpRequestMessage request)
        {
            if (!HostingEnvironment.IsDevelopment())
            {
                var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(Configuration.Identifier);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}
