using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EmployerAccounts.Queries
{
    public class GetAccountsQuery : IRequest<GetAccountsQueryResult>
    {
        public string UserId { get ; set ; }
        public string Email { get; set; }
    }
}