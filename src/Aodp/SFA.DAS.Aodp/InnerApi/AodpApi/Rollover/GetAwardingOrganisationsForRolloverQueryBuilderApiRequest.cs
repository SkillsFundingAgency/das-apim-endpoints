using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

public class GetAwardingOrganisationsForRolloverQueryBuilderApiRequest(RolloverQueryBuilderRequest data) : IPostApiRequest
{
    public string PostUrl => "api/rollover/querybuilder/awardingorganisations";

    public object Data { get; set; } = data;
}
