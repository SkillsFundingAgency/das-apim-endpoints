using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostSkillsAndStrengthsApiRequest
{
    public Guid CandidateId { get; set; }
    public string SkillsAndStrengths { get; set; }
}
