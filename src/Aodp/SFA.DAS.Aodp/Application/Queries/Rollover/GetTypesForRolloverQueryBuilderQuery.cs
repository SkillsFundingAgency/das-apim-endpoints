using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetTypesForRolloverQueryBuilderQuery(RolloverQueryBuilderTypesRequest filters)
    : IRequest<BaseMediatrResponse<GetTypesForRolloverQueryBuilderQueryResponse>>
{
    public RolloverQueryBuilderTypesRequest Filters { get; } = filters;
}