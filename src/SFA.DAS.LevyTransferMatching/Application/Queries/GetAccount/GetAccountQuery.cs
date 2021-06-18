using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount
{
    public class GetAccountQuery : IRequest<GetAccountResult>
    {
        public string EncodedAccountId { get; set; }
    }
}