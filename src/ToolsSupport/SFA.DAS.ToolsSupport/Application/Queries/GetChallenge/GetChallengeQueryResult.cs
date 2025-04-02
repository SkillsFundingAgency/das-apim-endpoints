using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Models;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetChallenge;

public class GetChallengeQueryResult
{
    public Account Account { get; set; } = new Account { LegalEntities = [], PayeSchemes = [] };

    public List<int> Characters { get; set; } = [];

    public SearchResponseCodes StatusCode { get; set; }
}
