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
            IWebHostEnvironment hostingEnvironment,
            IAzureClientCredentialHelper azureClientCredentialHelper)
            : base(httpClientFactory, apiConfiguration, hostingEnvironment, azureClientCredentialHelper)
        {
        }

        protected override Task AddAuthenticationHeader()
        {
            return Task.CompletedTask;
        }
    }
}