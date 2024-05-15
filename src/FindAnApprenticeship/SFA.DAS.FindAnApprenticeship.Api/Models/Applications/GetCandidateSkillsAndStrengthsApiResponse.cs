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
    public string Support { get; set; }
    public Guid ApplicationId { get; set; }

    public static implicit operator AboutYouItem(GetCandidateSkillsAndStrengthsQueryResult source)
    {
        return new AboutYouItem
        {
            SkillsAndStrengths = source.AboutYou.SkillsAndStrengths,
            Support = source.AboutYou.Support,
            ApplicationId = (Guid)source.AboutYou.ApplicationId
        };
    }
}
