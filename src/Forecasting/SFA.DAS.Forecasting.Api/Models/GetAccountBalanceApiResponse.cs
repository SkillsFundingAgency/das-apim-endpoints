using SFA.DAS.Forecasting.Application.Accounts.Queries.GetAccountBalance;

namespace SFA.DAS.Forecasting.Api.Models
{
    public class GetAccountBalanceApiResponse
    {
        public decimal Balance { get; set; }
        public decimal RemainingTransferAllowance { get; set; }
        public decimal StartingTransferAllowance { get; set; }
        public decimal TransferAllowance { get; set; }
        public int IsLevyPayer { get; set; }
        
        public static implicit operator GetAccountBalanceApiResponse(GetAccountBalanceQueryResult source)
        {
            if (source.AccountBalance == null)
            {
                return null;
            }
            return new GetAccountBalanceApiResponse
            {
                Balance = source.AccountBalance.Balance,
                TransferAllowance = source.AccountBalance.TransferAllowance,
                IsLevyPayer = source.AccountBalance.IsLevyPayer,
                RemainingTransferAllowance = source.AccountBalance.RemainingTransferAllowance,
                StartingTransferAllowance = source.AccountBalance.StartingTransferAllowance
            };
        }
    }
}