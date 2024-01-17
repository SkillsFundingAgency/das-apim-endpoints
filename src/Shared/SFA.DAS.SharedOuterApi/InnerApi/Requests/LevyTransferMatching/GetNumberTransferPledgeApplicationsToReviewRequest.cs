using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching
{
    public class GetNumberTransferPledgeApplicationsToReviewRequest : IGetApiRequest
    {
        private readonly long _accountId;

        public GetNumberTransferPledgeApplicationsToReviewRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetUrl => $"/accounts/{_accountId}/applications/pledge-applications-to-review";
    }
}