using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class GetAccountRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetAccountRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"/accounts/{AccountId}";
    }
}
