namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningData
{
    public class GetEditDraftApprenticeshipPriorLearningDataQueryResult
    {
        public int? PriceReduced { get; set; }
        public int? TrainingTotalHours { get; set; }
        public int? DurationReducedByHours { get; set; }
        public bool? IsDurationReducedByRpl { get; set; }
        public int? DurationReducedBy { get; set; } // by Weeks
    }
}
