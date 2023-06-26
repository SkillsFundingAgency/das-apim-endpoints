using MediatR;

namespace SFA.DAS.EmployerAan.Application.User.GetUserAccounts;

public record GetUserAccountsQuery(string UserId, string Email) : IRequest<GetUserAccountsQueryResult>;
