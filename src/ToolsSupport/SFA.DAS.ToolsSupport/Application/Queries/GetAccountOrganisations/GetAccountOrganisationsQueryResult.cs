using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetAccountOrganisations;

public class GetAccountOrganisationsQueryResult
{
    public IEnumerable<LegalEntity> LegalEntities { get; set; } = [];
}
