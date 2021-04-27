using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts
{
    public class GetAccountUsersRequest : IGetAllApiRequest
    {
        private readonly string _accountId;

        public GetAccountUsersRequest(string accountId)
        {
            _accountId = accountId;
        }

        public string GetAllUrl => $"api/accounts/internal/{_accountId}/legalentities/users";
    }
}