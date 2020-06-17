using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Infrastructure.Api
{
    public class AzureClientCredentialHelper :IAzureClientCredentialHelper
    {
        
        private readonly CoursesApiConfiguration _configuration;

        public AzureClientCredentialHelper (IOptions<CoursesApiConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }
        
        public async Task<string> GetAccessTokenAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_configuration.Identifier);
         
            return accessToken;
        }
        
    }
}