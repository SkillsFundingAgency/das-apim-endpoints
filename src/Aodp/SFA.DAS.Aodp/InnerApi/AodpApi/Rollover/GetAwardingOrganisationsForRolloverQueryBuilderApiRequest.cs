using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

public record GetAwardingOrganisationsForRolloverQueryBuilderApiRequest(RolloverQueryBuilderAwardingOrganisationsRequest Request) : IPostApiRequest
{
    public string PostUrl => "api/rollover/querybuilder/awardingorganisations";

    public object Data { get; set; } = Request;
}
