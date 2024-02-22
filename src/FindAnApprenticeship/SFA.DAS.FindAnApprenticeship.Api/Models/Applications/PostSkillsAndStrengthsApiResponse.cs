using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;
using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostSkillsAndStrengthsApiResponse
{
    public Guid Id { get; set; }

    public static implicit operator PostSkillsAndStrengthsApiResponse(CreateSkillsAndStrengthsCommandResult source)
    {
        return new PostSkillsAndStrengthsApiResponse
        {
            Id = source.Id
        };
    }
}
