using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

public record GetSectorSubjectAreaForRolloverQueryBuilderApiRequest(RolloverQueryBuilderSectorSubjectAreaRequest Request) : IPostApiRequest
{
    public object Data { get; set; } = Request;

    public string PostUrl => "api/rollover/querybuilder/sectorsubjectarea";
}