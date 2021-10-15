using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications
{
    public class GetApplicationRequest : IGetApiRequest
    {
        private readonly int? _pledgeId;
        private readonly int _applicationId;

        public GetApplicationRequest(int applicationId)
        {
            _applicationId = applicationId;
        }

        public GetApplicationRequest(int pledgeId, int applicationId)
        {
            _pledgeId = pledgeId;
            _applicationId = applicationId;
        }

        public string GetUrl
        {
            get
            {
                if (_pledgeId.HasValue)
                {
                    return $"pledges/{_pledgeId.Value}/applications/{_applicationId}";
                }
                else
                {
                    return $"applications/{_applicationId}";
                }
            }
        }
    }
}
