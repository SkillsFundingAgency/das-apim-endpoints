using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class ManagedIdentityApiHandler : DelegatingHandler
    {
        private readonly string _managedIdentifier;

        public ManagedIdentityApiHandler(string managedIdentifier) 
        {
            _managedIdentifier = managedIdentifier;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_managedIdentifier) && !request.Headers.Contains("authentication"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());
            }
                            
            return await base.SendAsync(request, cancellationToken);
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_managedIdentifier);

            return accessToken;
        }
    }
}
