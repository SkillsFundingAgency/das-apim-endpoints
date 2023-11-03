using MediatR;

namespace SFA.DAS.Apprenticeships.Application.EmployerAccounts.Queries
{
    public class GetAccountsQuery : IRequest<GetAccountsQueryResult>
    {
        public string UserId { get ; set ; }
        public string Email { get; set; }
    }
}