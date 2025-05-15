using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities
{
    public sealed record GetAllAccountLegalEntitiesQuery(long AccountId, int PageNumber, int PageSize, string SortColumn, bool IsAscending)
        : IRequest<GetAllAccountLegalEntitiesQueryResult>;
}