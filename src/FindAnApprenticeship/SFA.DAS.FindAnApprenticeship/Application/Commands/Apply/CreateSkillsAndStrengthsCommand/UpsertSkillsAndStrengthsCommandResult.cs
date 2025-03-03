using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;
public class UpsertSkillsAndStrengthsCommandResult
{
    public Guid Id { get; set; }
    public Domain.Models.Application Application { get; set; }
}
