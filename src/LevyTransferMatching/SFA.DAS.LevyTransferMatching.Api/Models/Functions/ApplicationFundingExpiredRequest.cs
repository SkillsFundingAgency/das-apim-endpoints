namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions
{
    public class ApplicationFundingExpiredRequest
    {
        public required int PledgeId { get; set; }
        public required int ApplicationId { get; set; }
        public required int Amount { get; set; }
    }
}