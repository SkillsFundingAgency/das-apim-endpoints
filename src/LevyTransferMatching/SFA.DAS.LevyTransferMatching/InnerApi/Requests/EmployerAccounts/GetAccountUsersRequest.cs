using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.EmployerAccounts
{
    public class GetAccountUsersRequest : IGetApiRequest
    {
        private readonly long _accountId;

        public GetAccountUsersRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetUrl => $"api/accounts/internal/{_accountId}/users";
    }
}
