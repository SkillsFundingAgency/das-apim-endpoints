using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetExpectedSkillsAndStrengths;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetExpectedSkillsAndStrengthsApiResponse
{
    public Guid ApplicationId { get; set; }
    public string Employer { get; set; }
    public IEnumerable<string> ExpectedSkillsAndStrengths { get; set; }
    public bool? IsSectionCompleted { get; set; }

    public static implicit operator GetExpectedSkillsAndStrengthsApiResponse(GetExpectedSkillsAndStrengthsQueryResult source)
    {
        return new GetExpectedSkillsAndStrengthsApiResponse
        {
            ApplicationId = source.ApplicationId,
            Employer = source.Employer,
            ExpectedSkillsAndStrengths = source.ExpectedSkillsAndStrengths,
            IsSectionCompleted = source.IsSectionCompleted
        };
    }
}
