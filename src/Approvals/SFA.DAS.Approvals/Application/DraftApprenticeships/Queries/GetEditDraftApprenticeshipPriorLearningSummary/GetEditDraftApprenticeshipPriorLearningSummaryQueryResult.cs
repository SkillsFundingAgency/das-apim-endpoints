namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningSummary
{
    public class GetEditDraftApprenticeshipPriorLearningSummaryQueryResult
    {
        public int? TrainingTotalHours { get; set; }
        public int? DurationReducedByHours { get; set; }
        public int? CostBeforeRpl { get; set; }
        public int? PriceReducedBy { get; set; }
        public int? FundingBandMaximum { get; set; }
        public decimal? PercentageOfPriorLearning { get; set; }
        public decimal? MinimumPercentageReduction { get; set; }
        public int? MinimumPriceReduction { get; set; }
        public bool? RplPriceReductionError { get; set; }
        public int? TotalCost { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool HasStandardOptions { get; set; }

    }
}
