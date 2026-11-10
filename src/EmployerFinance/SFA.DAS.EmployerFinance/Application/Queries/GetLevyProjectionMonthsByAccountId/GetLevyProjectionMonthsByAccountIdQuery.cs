using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionMonthsByAccountId;

public sealed record GetLevyProjectionMonthsByAccountIdQuery(long AccountId)
    : IRequest<GetLevyProjectionMonthsByAccountIdQueryResult>;
