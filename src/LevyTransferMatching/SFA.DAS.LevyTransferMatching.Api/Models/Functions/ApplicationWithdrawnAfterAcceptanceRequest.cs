namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions
{
    public class ApplicationWithdrawnAfterAcceptanceRequest
    {
        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
        public int Amount { get; set; }
    }
}
