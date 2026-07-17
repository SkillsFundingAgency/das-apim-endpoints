using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetSectorSubjectAreaForRolloverQueryBuilderQuery(RolloverQueryBuilderSectorSubjectAreaRequest filters)
    : IRequest<BaseMediatrResponse<GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse>>
{
    public RolloverQueryBuilderSectorSubjectAreaRequest Filters { get; } = filters;
}
