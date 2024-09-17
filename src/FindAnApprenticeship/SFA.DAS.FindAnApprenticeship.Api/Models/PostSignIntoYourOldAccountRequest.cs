using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public record PostSignIntoYourOldAccountRequest
    {
        public Guid CandidateId { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}