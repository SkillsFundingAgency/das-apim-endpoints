using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsStatusByAccountId;

public sealed record GetLevyProjectionsStatusByAccountIdQuery(long AccountId)
    : IRequest<GetLevyProjectionsStatusByAccountIdQueryResult>;
