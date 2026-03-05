using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationsByOrganisationType;

public class GetGetBlockedOrganisationsByOrganisationTypeQueryResult
{
    public List<GetBlockedOrganisationResponse> BlockedOrganisations { get; set; } = [];
}