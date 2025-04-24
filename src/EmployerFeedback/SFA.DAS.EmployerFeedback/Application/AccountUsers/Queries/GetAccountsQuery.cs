using MediatR;

namespace SFA.DAS.EmployerFeedback.Application.AccountUsers.Queries
{
    public class GetAccountsQuery : IRequest<GetAccountsQueryResult>
    {
        public string UserId { get ; set ; }
        public string Email { get; set; }
    }
}