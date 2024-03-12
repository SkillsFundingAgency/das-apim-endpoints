using MediatR;
using System;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationDisabilityConfidence;

public record PatchApplicationDisabilityConfidenceCommand : IRequest<PatchApplicationDisabilityConfidenceCommandResponse>
{
    public Guid ApplicationId { set; get; }
    public Guid CandidateId { set; get; }
    public SectionStatus DisabilityConfidenceStatus { get; set; }
}