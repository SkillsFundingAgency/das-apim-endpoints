using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments
{
    public class GetEmployerCohortsReadyForApprovalRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetEmployerCohortsReadyForApprovalRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/{AccountId}/ready-for-approval";
    }
}