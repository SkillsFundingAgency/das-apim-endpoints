using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.AccountUsers.Queries
{
    public class GetAccountsQuery : IRequest<GetAccountsQueryResult>
    {
        public string UserId { get ; set ; }
        public string Email { get; set; }
    }
}