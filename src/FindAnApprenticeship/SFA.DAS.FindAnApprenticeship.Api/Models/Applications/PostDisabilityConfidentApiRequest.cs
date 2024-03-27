using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class PostDisabilityConfidentApiRequest
    {
        public Guid CandidateId { get; set; }
        public bool ApplyUnderDisabilityConfidentScheme { get; set; }
    }
}
