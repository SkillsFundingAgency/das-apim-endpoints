using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProviderAdditionalStandardsRequest : IGetApiRequest
    {
        private readonly int _providerId;

        public GetProviderAdditionalStandardsRequest(int providerId)
        {
            _providerId = providerId;
        }

        public string GetUrl => $"api/providers/{_providerId}/courses";
    }
}