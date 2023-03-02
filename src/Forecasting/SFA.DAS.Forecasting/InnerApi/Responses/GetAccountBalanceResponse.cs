using System.Collections.Generic;

namespace SFA.DAS.Forecasting.InnerApi.Responses
{
    public class GetAccountBalanceResponse
    {
        public long AccountId { get; set; }
        public decimal Balance { get; set; }
        public decimal RemainingTransferAllowance { get; set; }
        public decimal StartingTransferAllowance { get; set; }
        public decimal TransferAllowance { get; set; }
        public int IsLevyPayer { get; set; }
    }
}