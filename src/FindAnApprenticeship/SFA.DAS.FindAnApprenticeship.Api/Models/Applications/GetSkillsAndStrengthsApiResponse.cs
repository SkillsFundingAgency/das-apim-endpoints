using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetSkillsAndStrengths;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetSkillsAndStrengthsApiResponse
{
    public Guid ApplicationId { get; set; }
    public string Employer { get; set; }
    public IEnumerable<string> ExpectedSkillsAndStrengths { get; set; }

    public static implicit operator GetSkillsAndStrengthsApiResponse(GetSkillsAndStrengthsQueryResult source)
    {
        return new GetSkillsAndStrengthsApiResponse
        {
            ApplicationId = source.ApplicationId,
            Employer = source.Employer,
            ExpectedSkillsAndStrengths = source.ExpectedSkillsAndStrengths
        };
    }
}
