using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetCandidateSkillsAndStrengthsItemApiResponse
{
    public AboutYouItem AboutYou { get; set; }
}

public class AboutYouItem
{
    public string SkillsAndStrengths { get; set; }
    public string Improvements { get; set; }
    public string HobbiesAndInterests { get; set; }
    public string Support { get; set; }
    public Guid ApplicationId { get; set; }
}
