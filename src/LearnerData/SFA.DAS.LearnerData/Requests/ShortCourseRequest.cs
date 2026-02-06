namespace SFA.DAS.LearnerData.Requests
{
    public class ShortCourseRequest
    {
        public ShortCourseLearnerRequestDetails Learner { get; set; }

        public ShortCourseDelivery Delivery { get; set; }
    }

    public class ShortCourseLearnerRequestDetails : LearnerRequestDetails
    {
        public long Uln { get; }
    }

    public class ShortCourseDelivery
    {
        public List<ShortCourseOnProgramme> OnProgramme { get; set; }
    }

    public class ShortCourseOnProgramme
    {
        public string CourseCode { get; set; }
        public string? AgreementId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public List<LearningSupportRequestDetails> LearningSupport { get; set; }
        public DateTime? PauseDate { get; set; }
        public int? AimSequenceNumber { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public Milestone[] Milestones { get; set; }
    }

    public enum Milestone
    {
        ThirtyPercentLearningComplete,
        LearningComplete
    }
}
