using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public record PostUpdateVolunteeringOrWorkExperienceApiRequest
    {
        public required Guid CandidateId { get; set; }
        public string EmployerName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
