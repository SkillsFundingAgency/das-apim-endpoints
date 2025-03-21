using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests
{
    public class GetApprenticeshipRegistrationRequest : IGetApiRequest
    {
        private readonly long _apprenticeshipId;

        public GetApprenticeshipRegistrationRequest(long apprenticeshipId)
        {
            _apprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"approvals/{_apprenticeshipId}/registration";
    }
}