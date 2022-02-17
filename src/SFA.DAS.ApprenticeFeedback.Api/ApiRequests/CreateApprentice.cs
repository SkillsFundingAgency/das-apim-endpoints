using System;

namespace SFA.DAS.ApprenticeFeedback.Api.ApiRequests
{
    public class CreateApprentice
    {
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
    }
}
