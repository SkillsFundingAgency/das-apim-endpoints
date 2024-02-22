using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationSkillsAndStrengths;
public class PatchApplicationSkillsAndStrengthsCommand : IRequest<PatchApplicationSkillsAndStrengthsCommandResult>
{
    public Guid ApplicationId { set; get; }
    public Guid CandidateId { set; get; }
    public SectionStatus SkillsAndStrengthsSectionStatus { get; set; }
}
