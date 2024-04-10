using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin
{
    public class ApprenticeLoginClient : InternalApiClient<ApprenticeLoginConfiguration>
    {
        public ApprenticeLoginClient(
            IHttpClientFactory httpClientFactory,
            ApprenticeLoginConfiguration apiConfiguration,
            IAzureClientCredentialHelper azureClientCredentialHelper,
            ILogger<InternalApiClient<ApprenticeLoginConfiguration>> logger)
            : base(httpClientFactory, apiConfiguration, azureClientCredentialHelper, logger)
        {
        }

        protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            return Task.CompletedTask;
        }
    }
}