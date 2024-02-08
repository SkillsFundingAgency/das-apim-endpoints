using System;

namespace SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange
{
    public class GetPendingPriceChangeResponse
    {
        public bool HasPendingPriceChange { get; set; }
        public PendingPriceChange PendingPriceChange { get; set; }
    }

    public class PendingPriceChange
    {
		public decimal OriginalTrainingPrice { get; set; }
		public decimal OriginalAssessmentPrice { get; set; }
		public decimal OriginalTotalPrice { get; set; }
		public decimal? PendingTrainingPrice { get; set; }
		public decimal? PendingAssessmentPrice { get; set; }
		public decimal PendingTotalPrice { get; set; }
		public DateTime EffectiveFrom { get; set; }
		public string Reason { get; set; }
	}
}
