using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

public class GetQualificationVersionsForRolloverQueryBuilderApiRequest(RolloverQueryBuilderRequest data) : IPostApiRequest
{
    public string PostUrl => "api/rollover/querybuilder/qualificationversions";

    public object Data { get; set; } = data;
}
