using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetOverlappingTrainingDateResponse
    {
        public IReadOnlyCollection<ApprenticeshipOverlappingTrainingDateRequest> OverlappingTrainingDateRequest { get; set; }

        public class ApprenticeshipOverlappingTrainingDateRequest
        {
            public long Id { get; set; }
            public long DraftApprenticeshipId { get; set; }
            public long PreviousApprenticeshipId { get; set; }
            public short? ResolutionType { get; set; }
            public short Status { get; set; }
            public DateTime? ActionedOn { get; set; }
        }
    }
}