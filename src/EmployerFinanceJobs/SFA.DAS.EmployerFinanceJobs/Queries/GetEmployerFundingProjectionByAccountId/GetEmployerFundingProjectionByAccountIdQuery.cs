using MediatR;

namespace SFA.DAS.EmployerFinanceJobs.Queries.GetEmployerFundingProjectionByAccountId;

public sealed record GetEmployerFundingProjectionByAccountIdQuery(long AccountId)
    : IRequest<GetEmployerFundingProjectionByAccountIdQueryResult>;