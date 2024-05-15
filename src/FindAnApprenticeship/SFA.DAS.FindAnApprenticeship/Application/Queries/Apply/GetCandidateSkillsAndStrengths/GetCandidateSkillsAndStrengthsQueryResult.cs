using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
public class GetCandidateSkillsAndStrengthsQueryResult
{
    public AboutYouItem AboutYou { get; set; }

    public static implicit operator GetCandidateSkillsAndStrengthsQueryResult(ApiResponse<PutUpsertAboutYouItemApiResponse> source)
    {
        return new GetCandidateSkillsAndStrengthsQueryResult
        {
            AboutYou = source is null ? null : (AboutYouItem)source.Body
        };
    }
    public static implicit operator GetCandidateSkillsAndStrengthsQueryResult(ApiResponse<GetAboutYouItemApiResponse> source)
    {
        return new GetCandidateSkillsAndStrengthsQueryResult
        {
            AboutYou = source is null ? null : (AboutYouItem)source.Body.AboutYou
        };
    }
}

public class AboutYouItem
{
    public string? SkillsAndStrengths { get; set; }
    public string? Support { get; set; }
    public Guid? ApplicationId { get; set; }

    public static implicit operator AboutYouItem(PutUpsertAboutYouItemApiResponse source)
    {
        return new AboutYouItem
        {
            SkillsAndStrengths = source?.Strengths,
            Support = source?.Support,
            ApplicationId = source?.ApplicationId
        };
    }
    public static implicit operator AboutYouItem(SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.AboutYouItem source)
    {
        return new AboutYouItem
        {
            SkillsAndStrengths = source?.SkillsAndStrengths,
            Support = source?.Support,
            ApplicationId = source?.ApplicationId
        };
    }
}
