using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostSkillsAndStrengthsApiRequest
{
    public Guid CandidateId { get; set; }
    public string SkillsAndStrengths { get; set; }
    public SectionStatus SkillsAndStrengthsSectionStatus { get; set; }
}
