using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Infrastructure.Api
{
    public class ApiClient : ApiClientBase, IApiClient
    {
        public ApiClient(IOptions<CoursesApiConfiguration> configuration, HttpClient httpClient, IHostingEnvironment hostingEnvironment) 
            : base(configuration, httpClient, hostingEnvironment)
        {
            
        }

        public override async Task<string> Ping()
        {
            var pingUrl = Configuration.Url;

            pingUrl += pingUrl.EndsWith("/") ? "ping" : "/ping";

            var response = await HttpClient.GetAsync(pingUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return result;
        }

        protected override async Task<string> GetAccessTokenAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(Configuration.Identifier);
            return accessToken;
        }
    }
}
