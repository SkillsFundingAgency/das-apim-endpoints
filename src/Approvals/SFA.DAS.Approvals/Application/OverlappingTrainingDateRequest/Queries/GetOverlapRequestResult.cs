using System;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries
{
    public class GetOverlapRequestResult
    {
        public long? DraftApprenticeshipId { get; set; }
        public long? PreviousApprenticeshipId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
