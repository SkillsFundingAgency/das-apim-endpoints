using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Infrastructure.Api
{
    public abstract class ApiClientBase
    {
        protected readonly HttpClient HttpClient;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
        protected readonly CoursesApiConfiguration Configuration;

        protected ApiClientBase(
            IOptions<CoursesApiConfiguration> configuration,
            HttpClient httpClient, IHostingEnvironment hostingEnvironment,
            IAzureClientCredentialHelper azureClientCredentialHelper)
        {
            HttpClient = httpClient;
            _hostingEnvironment = hostingEnvironment;
            _azureClientCredentialHelper = azureClientCredentialHelper;
            Configuration = configuration.Value;
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            await AddAuthenticationHeader();

            request.BaseUrl = Configuration.Url;
            var response = await HttpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        private async Task AddAuthenticationHeader()
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync();
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);    
            }
        }

        public abstract Task<string> Ping();

    }
}
