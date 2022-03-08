using MediatR;

namespace SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountQuery
{
    public class GetAccountQuery : IRequest<GetAccountResult>
    {
        public long AccountId { get; set; }
    }
}