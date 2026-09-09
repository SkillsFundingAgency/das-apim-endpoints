using MediatR;

namespace SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfilesByAccountId;

public sealed record GetEmployerProfilesByAccountIdQuery(long AccountId)
    : IRequest<GetEmployerProfilesByAccountIdQueryResult>;