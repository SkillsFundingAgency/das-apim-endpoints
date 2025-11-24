using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.ManagingStandards.Requests
{
    public class GetProviderCourseStandardsRequest : IGetApiRequest
    {
        public readonly long ProviderId;
        public readonly int LarsCode;

        public GetProviderCourseStandardsRequest(long providerId, int larsCode)
        {
            ProviderId = providerId;
            LarsCode = larsCode;
        }

        public string GetUrl => $"api/providers/{ProviderId}/courses/{LarsCode}";
    }
}
