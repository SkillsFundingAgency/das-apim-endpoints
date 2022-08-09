using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi
{
    public class GetProviderRequest : IGetApiRequest
    {
        private readonly long _providerId;

        public GetProviderRequest(long providerId)
            => _providerId = providerId;

        public string GetUrl => $"api/providers/{_providerId}";
    }
}
