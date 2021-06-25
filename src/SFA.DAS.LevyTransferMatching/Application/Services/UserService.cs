using SFA.DAS.LevyTransferMatching.Configuration;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IEmployerAccountsApiClient<EmployerAccountsConfiguration> _employerAccountsApiClient;

        public UserService(IEmployerAccountsApiClient<EmployerAccountsConfiguration> employerAccountsApiClient)
        {
            _employerAccountsApiClient = employerAccountsApiClient;
        }

        public async Task<IEnumerable<Account>> GetUserAccounts(string userId)
        {
            var response = await _employerAccountsApiClient.GetAll<Account>(new GetUserAccountsRequest(userId));

            return response;
        }
    }
}