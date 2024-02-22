using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;
public class CreateSkillsAndStrengthsCommand : IRequest<CreateSkillsAndStrengthsCommandResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public string SkillsAndStrengths { get; set; }
}
