using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.ManagingStandards.Requests
{
    public class GetProviderStandardsRequest : IGetApiRequest
    {
        public readonly long ProviderId;

        public GetProviderStandardsRequest(long providerId)
        {
            ProviderId = providerId;
        }

        public string GetUrl => $"api/providers/{ProviderId}/courses";
    }
}
