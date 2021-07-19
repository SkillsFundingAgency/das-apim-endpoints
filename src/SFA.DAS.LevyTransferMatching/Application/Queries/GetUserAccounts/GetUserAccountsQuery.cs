using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetUserAccounts
{
    public class GetUserAccountsQuery : IRequest<GetUserAccountsResult>
    {
        public string UserId { get; set; }
    }
}