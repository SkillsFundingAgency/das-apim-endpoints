using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Accounts
{
    public class GetAccountRequest : IGetApiRequest
    {
        private readonly string _encodedAccountId;

        public GetAccountRequest(string encodedAccountId)
        {
            _encodedAccountId = encodedAccountId;
        }

        public string GetUrl => $"api/accounts/{_encodedAccountId}";
    }
}
