namespace SFA.DAS.RecruitQa.Application.Profanity.GetProfanity;

public record GetProfanityListQueryResult
{
    public List<string> ProfanityList { get; init; } = [];

    public static GetProfanityListQueryResult FromInnerApiResponse(List<string> response)
    {
        return new GetProfanityListQueryResult
        {
            ProfanityList = response
        };
    }
}