namespace SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange
{
    public class GetPendingPriceChangeResponse
    {
        public bool HasPendingPriceChange { get; set; }
        public PendingPriceChange PendingPriceChange { get; set; }
    }

    public class PendingPriceChange
    {
        public decimal Cost { get; set; }
        public decimal? TrainingPrice { get; set; }
        public decimal? EndPointAssessmentPrice { get; set; }
    }
}
