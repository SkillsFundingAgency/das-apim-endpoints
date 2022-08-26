using SFA.DAS.Approvals.Enums;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class ValidateUlnOverlapOnStartDateResponse
    {
        public long? HasOverlapWithApprenticeshipId { get; set; }
        public bool HasStartDateOverlap { get; set; }
    }
}
