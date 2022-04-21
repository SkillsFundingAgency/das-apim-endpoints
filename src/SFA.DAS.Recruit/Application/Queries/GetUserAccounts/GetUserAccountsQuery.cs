using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetUserAccounts
{
    public class GetUserAccountsQuery : IRequest<GetUserAccountsQueryResult>
    {
        public string UserId { get; set; }
    }
}