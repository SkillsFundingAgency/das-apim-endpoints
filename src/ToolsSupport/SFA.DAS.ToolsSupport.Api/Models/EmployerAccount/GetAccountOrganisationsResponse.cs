using SFA.DAS.ToolsSupport.Application.Queries.GetAccountOrganisations;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;

public class GetAccountOrganisationsResponse
{
    public IEnumerable<LegalEntity> LegalEntities { get; set; } = [];

    public static explicit operator GetAccountOrganisationsResponse(GetAccountOrganisationsQueryResult source)
    {
        if (source == null) return new GetAccountOrganisationsResponse { LegalEntities = [] };

        return new GetAccountOrganisationsResponse
        {
            LegalEntities = source.LegalEntities
        };
    }
}
