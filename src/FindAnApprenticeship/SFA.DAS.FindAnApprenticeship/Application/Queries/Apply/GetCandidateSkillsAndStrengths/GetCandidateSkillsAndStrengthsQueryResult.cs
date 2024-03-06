using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
public class GetCandidateSkillsAndStrengthsQueryResult
{
    public AboutYouItem AboutYou { get; set; }

    public static implicit operator GetCandidateSkillsAndStrengthsQueryResult(GetCandidateSkillsAndStrengthsItemApiResponse source)
    {
        return new GetCandidateSkillsAndStrengthsQueryResult
        {
            AboutYou = source.AboutYou is null ? null : (AboutYouItem)source
        };
    }
}

public class AboutYouItem
{
    public string? SkillsAndStrengths { get; set; }
    public string? Improvements { get; set; }
    public string? HobbiesAndInterests { get; set; }
    public string? Support { get; set; }
    public Guid? ApplicationId { get; set; }

    public static implicit operator AboutYouItem(GetCandidateSkillsAndStrengthsItemApiResponse source)
    {
        return new AboutYouItem
        {
            SkillsAndStrengths = source.AboutYou.SkillsAndStrengths,
            Improvements = source.AboutYou.Improvements,
            HobbiesAndInterests = source.AboutYou.HobbiesAndInterests,
            Support = source.AboutYou.Support,
            ApplicationId = source.AboutYou.ApplicationId
        };
    }
}
