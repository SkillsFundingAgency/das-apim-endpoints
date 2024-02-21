using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetSkillsAndStrengths;
public class GetSkillsAndStrengthsQueryResult
{
    public Guid ApplicationId { get; set; }
    public string Employer { get; set; }
    public IEnumerable<string> ExpectedSkillsAndStrengths { get; set; }
}
