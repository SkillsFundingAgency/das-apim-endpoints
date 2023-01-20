using MediatR;

namespace SFA.DAS.Forecasting.Application.Accounts.Queries.GetAccountBalance
{
    public class GetAccountBalanceQuery : IRequest<GetAccountBalanceQueryResult>
    {
        public string AccountId { get; set; }
    }
}