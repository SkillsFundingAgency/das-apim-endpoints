using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Accounts
{
    public class GetAccountRequest : IGetApiRequest
    {
        private readonly string _encodedAccountId;
        private readonly long? _accountId;

        public GetAccountRequest(string encodedAccountId)
        {
            _encodedAccountId = encodedAccountId;
        }

        public GetAccountRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetUrl => _accountId.HasValue ? $"api/accounts/internal/{_accountId}" : $"api/accounts/{_encodedAccountId}";
    }
}
