using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;

public sealed record GetLevySummaryByAccountIdQuery(long AccountId)
    : IRequest<GetLevySummaryByAccountIdQueryResult>;