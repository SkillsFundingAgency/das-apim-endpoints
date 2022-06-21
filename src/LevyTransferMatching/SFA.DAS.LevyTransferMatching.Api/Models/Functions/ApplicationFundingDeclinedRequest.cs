namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions
{
    public class ApplicationFundingDeclinedRequest
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public int Amount { get; set; }
    }
}