using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications
{
    public class GetApplicationRequest : IGetApiRequest
    {
        private readonly int _pledgeId;
        private readonly int _applicationId;

        public GetApplicationRequest(int pledgeId, int applicationId)
        {
            _pledgeId = pledgeId;
            _applicationId = applicationId;
        }

        public string GetUrl => $"pledges/{_pledgeId}/applications/{_applicationId}";
    }
}
