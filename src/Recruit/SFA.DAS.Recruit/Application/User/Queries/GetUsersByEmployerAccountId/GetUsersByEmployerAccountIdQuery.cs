namespace SFA.DAS.Recruit.Application.User.Queries.GetUsersByEmployerAccountId;

public sealed record GetUsersByEmployerAccountIdQuery(long EmployerAccountId)
    : MediatR.IRequest<GetUsersByEmployerAccountIdQueryResult>;