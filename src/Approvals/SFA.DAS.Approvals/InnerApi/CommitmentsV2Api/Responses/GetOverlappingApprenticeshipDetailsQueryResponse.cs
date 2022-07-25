using SFA.DAS.Approvals.Enums;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetOverlappingApprenticeshipDetailsResponse
    {
        public long ApprenticeshipId { get; set; }
        public ApprenticeshipStatus Status { get; set; }
    }
}
