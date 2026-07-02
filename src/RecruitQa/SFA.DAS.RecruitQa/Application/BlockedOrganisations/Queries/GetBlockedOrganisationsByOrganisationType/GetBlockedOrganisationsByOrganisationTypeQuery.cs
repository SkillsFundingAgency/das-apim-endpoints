using MediatR;

namespace SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationsByOrganisationType;

public class GetBlockedOrganisationsByOrganisationTypeQuery : IRequest<GetGetBlockedOrganisationsByOrganisationTypeQueryResult>
{
    public string OrganisationType { get; set; }
}