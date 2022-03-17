using MediatR;

namespace SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountUsersQuery
{
    public class GetAccountUsersQuery : IRequest<GetAccountUsersResult>
    {
        public string HashedAccountId { get; set; }
    }
}