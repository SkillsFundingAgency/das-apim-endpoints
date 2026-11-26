using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
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