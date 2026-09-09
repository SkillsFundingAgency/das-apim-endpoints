using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetQualificationVersionsForRolloverQueryBuilderQuery(RolloverQueryBuilderRequest filters)
    : IRequest<BaseMediatrResponse<GetQualificationVersionsForRolloverQueryBuilderQueryResponse>>
{
    public RolloverQueryBuilderRequest Filters { get; } = filters;
}
