namespace SFA.DAS.Approvals.Api.Models.DraftApprenticeships
{
    public class AddPriorLearningDataRequest
    {
        public int? TrainingTotalHours { get; set; }
        public int? DurationReducedByHours { get; set; }
        public bool? IsDurationReducedByRpl { get; set; }
        public int? DurationReducedBy { get; set; }
        public int? PriceReducedBy { get; set; }
    }
}
