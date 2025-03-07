using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetAccountOrganisations;

public class GetAccountOrganisationsQuery : IRequest<GetAccountOrganisationsQueryResult>
{
    public long AccountId { get; set; }
}
