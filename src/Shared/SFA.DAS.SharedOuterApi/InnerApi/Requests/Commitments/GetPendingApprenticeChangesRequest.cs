using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments
{
    public class GetPendingApprenticeChangesRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetPendingApprenticeChangesRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/{AccountId}/pending-apprentice-changes";
    }
}