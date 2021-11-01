using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetPledgeApplicationRequest : IGetApiRequest
    {
        private readonly int _applicationId;

        public GetPledgeApplicationRequest(int applicationId)
        {
            _applicationId = applicationId;
        }

        public string GetUrl => $"applications/{_applicationId}";
    }
}
