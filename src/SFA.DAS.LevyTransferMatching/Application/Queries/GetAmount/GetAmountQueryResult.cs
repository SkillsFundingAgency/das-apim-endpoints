namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAmount
{
    public class GetAmountQueryResult
    {
        public string DasAccountName { get; set; }
        public decimal RemainingTransferAllowance { get; set; }
    }
}