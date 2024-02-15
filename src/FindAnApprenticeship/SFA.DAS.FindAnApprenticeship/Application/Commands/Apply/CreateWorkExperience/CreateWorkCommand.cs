using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;

public record CreateWorkCommand : IRequest<CreateWorkCommandResponse>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public string CompanyName { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}