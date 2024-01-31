using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Apprenticeships.InnerApi
{
    public class GetProviderRequest : IGetApiRequest
    {
        public readonly long ProviderId;
        public string GetUrl => $"api/providers/{ProviderId}";

        public GetProviderRequest(long providerId)
        {
            ProviderId = providerId;
        }
    }
}
