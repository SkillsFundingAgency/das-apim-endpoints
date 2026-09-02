using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByHashedAccountId;

public sealed record GetLevySummaryByHashedAccountIdQuery(string HashedAccountId)
    : IRequest<GetLevySummaryByHashedAccountIdQueryResult>;