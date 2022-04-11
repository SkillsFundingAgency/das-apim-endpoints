using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetAccount
{
    public class GetAccountQuery : IRequest<GetAccountQueryResult>
    {
        public long AccountId { get; set; }
    }
}