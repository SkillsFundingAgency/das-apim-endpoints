using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class PostWhatInterestsYouApiRequest
    {
        public Guid CandidateId { get; set; }
        public string AnswerText { get; set; }
        public bool IsComplete { get; set; }
    }
}
