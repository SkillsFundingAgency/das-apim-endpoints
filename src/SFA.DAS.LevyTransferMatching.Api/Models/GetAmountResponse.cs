namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class GetAmountResponse
    {
        public string DasAccountName { get; set; }
        public decimal RemainingTransferAllowance { get; set; }
    }
}
