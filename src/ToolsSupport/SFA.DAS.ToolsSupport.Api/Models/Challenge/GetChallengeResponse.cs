using SFA.DAS.ToolsSupport.Application.Queries.GetChallenge;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Models;

namespace SFA.DAS.ToolsSupport.Api.Models.Challenge;

public class GetChallengeResponse
{
    public Account Account { get; set; } = new Account { LegalEntities = [], PayeSchemes = [] };

    public List<int> Characters { get; set; } = [];

    public SearchResponseCodes StatusCode { get; set; }

    public static explicit operator GetChallengeResponse(GetChallengeQueryResult source)
    {
        if (source == null) return new GetChallengeResponse();

        return new GetChallengeResponse
        {
            Account = source.Account,
            Characters = source.Characters,
            StatusCode = source.StatusCode
        };
    }
}
