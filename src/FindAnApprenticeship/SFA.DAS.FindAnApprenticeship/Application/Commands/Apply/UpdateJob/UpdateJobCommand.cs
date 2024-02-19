using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateJob
{
    public class UpdateJobCommand : IRequest
    {
        public Guid JobId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
