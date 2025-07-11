using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities
{
    public sealed record GetAllAccountLegalEntitiesQuery(
        string SearchTerm,
        List<long> AccountIds,
        int PageNumber,
        int PageSize,
        string SortColumn,
        bool IsAscending)
        : IRequest<GetAllAccountLegalEntitiesQueryResult>;
}