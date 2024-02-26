using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationVolunteeringAndWorkHistory;
public class PatchApplicationVolunteeringAndWorkExperienceCommand : IRequest<PatchApplicationVolunteeringAndWorkExperienceCommandResponse>
{
    public Guid ApplicationId { set; get; }
    public Guid CandidateId { set; get; }
    public SectionStatus VolunteeringAndWorkExperienceStatus { get; set; }
}
