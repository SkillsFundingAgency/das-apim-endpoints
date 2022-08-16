using SFA.DAS.EmployerAccounts.Application.Interfaces;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerAccounts.Application.Services
{
    public class EmployerAccountsService : IEmployerAccountsService
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public EmployerAccountsService(IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _accountsApiClient = accountsApiClient;
        }

        public async Task<bool> IsHealthy()
        {
            try
            {
                var status = await _accountsApiClient.GetResponseCode(new GetHealthRequest());
                return (status == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }
    }
}
