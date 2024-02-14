using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;

public record CreateWorkCommand : IRequest<CreateWorkCommandResponse>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public string EmployerName { get; set; }
    public string JobTitle { get; set; }
    public string JobDescription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}