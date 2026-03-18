using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

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
