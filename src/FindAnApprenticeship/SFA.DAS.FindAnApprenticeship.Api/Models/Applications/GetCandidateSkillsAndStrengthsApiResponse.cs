using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetCandidateSkillsAndStrengthsApiResponse
{
    public AboutYouItem AboutYou { get; set; }

    public static implicit operator GetCandidateSkillsAndStrengthsApiResponse(GetCandidateSkillsAndStrengthsQueryResult source)
    {
        if (source.AboutYou is null) return new GetCandidateSkillsAndStrengthsApiResponse();

        return new GetCandidateSkillsAndStrengthsApiResponse
        {
            AboutYou = (AboutYouItem)source
        };
    }
}

public class AboutYouItem
{
    public string SkillsAndStrengths { get; set; }
    public string Improvements { get; set; }
    public string HobbiesAndInterests { get; set; }
    public string Support { get; set; }
    public Guid ApplicationId { get; set; }

    public static implicit operator AboutYouItem(GetCandidateSkillsAndStrengthsQueryResult source)
    {
        return new AboutYouItem
        {
            SkillsAndStrengths = source.AboutYou.SkillsAndStrengths,
            Improvements = source.AboutYou.Improvements,
            HobbiesAndInterests = source.AboutYou.HobbiesAndInterests,
            Support = source.AboutYou.Support,
            ApplicationId = (Guid)source.AboutYou.ApplicationId
        };
    }
}
