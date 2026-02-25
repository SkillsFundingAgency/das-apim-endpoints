using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProviderAdditionalStandardsRequest : IGetApiRequest
    {
        private readonly int _providerId;
        public string Version => ApiVersionNumber.Two;
        public GetProviderAdditionalStandardsRequest(int providerId)
        {
            _providerId = providerId;
        }

        public string GetUrl => $"api/providers/{_providerId}/courses";
    }
}