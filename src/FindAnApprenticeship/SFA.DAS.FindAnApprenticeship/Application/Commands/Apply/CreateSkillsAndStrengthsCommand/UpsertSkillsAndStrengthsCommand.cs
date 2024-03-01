using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;
public class UpsertSkillsAndStrengthsCommand : IRequest<UpsertSkillsAndStrengthsCommandResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public string SkillsAndStrengths { get; set; }
    public SectionStatus SkillsAndStrengthsSectionStatus { get; set; }
}
