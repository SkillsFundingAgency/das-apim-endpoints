using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetAwardingOrganisationsForRolloverQueryBuilderQuery(RolloverQueryBuilderAwardingOrganisationsRequest filters)
    : IRequest<BaseMediatrResponse<GetAwardingOrganisationsForRolloverQueryBuilderQueryResponse>>
{
    public RolloverQueryBuilderAwardingOrganisationsRequest Filters { get; } = filters;
}
