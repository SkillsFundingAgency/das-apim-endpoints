using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetAccountRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetAccountRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/internal/{AccountId}";
    }
}