using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public sealed record GetLevyProjectionsByAccountIdQuery(long AccountId, int Months = 12)
    : IRequest<GetLevyProjectionsByAccountIdQueryResult>;