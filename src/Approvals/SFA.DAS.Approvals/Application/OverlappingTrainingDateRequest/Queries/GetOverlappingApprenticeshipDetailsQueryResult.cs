using SFA.DAS.Approvals.Enums;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries
{
    public class GetOverlappingApprenticeshipDetailsQueryResult
    {
        public long ApprenticeshipId { get; set; }
        public ApprenticeshipStatus Status { get; set; }
    }
}
