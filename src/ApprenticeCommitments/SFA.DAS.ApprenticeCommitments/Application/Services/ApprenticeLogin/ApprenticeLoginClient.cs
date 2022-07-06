using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
            IAzureClientCredentialHelper azureClientCredentialHelper)
            : base(httpClientFactory, apiConfiguration, azureClientCredentialHelper)
        {
        }

        protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            return Task.CompletedTask;
        }
    }
}