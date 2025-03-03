using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerAccounts.ExternalApi
{
    public class CompaniesHouseApiClient<T> : GetApiClient<T>, ICompaniesHouseApiClient<T> where T : ICompaniesHouseApiConfiguration
    {
        public CompaniesHouseApiClient(IHttpClientFactory httpClientFactory, T apiConfiguration) : base(httpClientFactory, apiConfiguration)
        {
        }

        protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            httpRequestMessage.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(Configuration.ApiKey))}");
        }
    }
}