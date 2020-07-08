using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Api
{
    public class ManagedIdentityApiHandler<T> : DelegatingHandler where T : IAzureManagedIdentityApiConfiguration
    {
        private readonly IAzureManagedIdentityApiConfiguration _configuration;

        public ManagedIdentityApiHandler(T configuration) 
        {
            _configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_configuration.Identifier) && !request.Headers.Contains("authentication"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());
            }
                            
            return await base.SendAsync(request, cancellationToken);
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_configuration.Identifier);

            return accessToken;
        }
    }
}
