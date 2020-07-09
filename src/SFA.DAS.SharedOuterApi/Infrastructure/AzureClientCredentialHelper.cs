using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class AzureClientCredentialHelper : IAzureClientCredentialHelper
    {
        public async Task<string> GetAccessTokenAsync(string identifier)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(identifier);
         
            return accessToken;
        }
        
    }
}