using MediatR;

namespace SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountQuery
{
    public class GetAccountQuery : IRequest<GetAccountResult>
    {
        public string HashedAccountId { get; set; }
    }
}