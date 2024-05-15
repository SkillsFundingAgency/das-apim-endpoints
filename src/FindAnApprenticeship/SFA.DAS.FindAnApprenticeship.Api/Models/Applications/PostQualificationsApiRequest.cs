using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class PostQualificationsApiRequest
    {
        public Guid CandidateId { get; set; }
        public bool IsComplete { get; set; }
    }
}
