using MediatR;

namespace SFA.DAS.EmployerPR.Application.UserAccounts.Queries.GetUserAccounts;
public record GetUserAccountsQuery(string UserId, string Email) : IRequest<GetUserAccountsQueryResult>;
