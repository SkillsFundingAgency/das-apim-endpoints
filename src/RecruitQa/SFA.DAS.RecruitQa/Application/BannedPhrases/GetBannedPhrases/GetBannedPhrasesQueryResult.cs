namespace SFA.DAS.RecruitQa.Application.BannedPhrases.GetBannedPhrases;

public class GetBannedPhrasesQueryResult
{
    public List<string> BannedPhraseList { get; init; } = [];

    public static GetBannedPhrasesQueryResult FromInnerApiResponse(List<string> response)
    {
        return new GetBannedPhrasesQueryResult
        {
            BannedPhraseList = response
        };
    }
}