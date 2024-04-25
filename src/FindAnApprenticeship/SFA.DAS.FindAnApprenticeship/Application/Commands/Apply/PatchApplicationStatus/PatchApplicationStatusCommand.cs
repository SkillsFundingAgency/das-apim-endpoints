using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationStatus;

public record PatchApplicationStatusCommand : IRequest<PatchApplicationStatusCommandResponse>
{
    public Guid ApplicationId { set; get; }
    public Guid CandidateId { set; get; }
    public ApplicationStatus Status { get; set; }
}