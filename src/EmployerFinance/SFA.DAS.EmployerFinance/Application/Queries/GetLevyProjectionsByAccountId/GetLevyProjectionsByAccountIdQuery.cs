using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public sealed record GetLevyProjectionsByAccountIdQuery(long AccountId, int Months = 6, bool TriggerRefresh = true)
    : IRequest<GetLevyProjectionsByAccountIdQueryResult>;