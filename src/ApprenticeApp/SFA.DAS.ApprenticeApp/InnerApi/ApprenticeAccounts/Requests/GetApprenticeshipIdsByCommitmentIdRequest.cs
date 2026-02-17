using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetApprenticeshipIdsByCommitmentIdRequest : IGetApiRequest
    {
        private long _commitmentId;

        public GetApprenticeshipIdsByCommitmentIdRequest(long commitmentId)
        {
            _commitmentId = commitmentId;
        }

        public string GetUrl => $"/api/apprenticeships/commitmentid/{_commitmentId}";
    }
}
