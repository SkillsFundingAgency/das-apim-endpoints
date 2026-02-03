using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LearnerData.Requests
{
    public class ShortCourseRequest
    {
        [Required]
        public LearnerRequestDetails Learner { get; set; }

        [Required]
        public ShortCourseDelivery Delivery { get; set; }
    }

    public class ShortCourseDelivery
    {
        [Required]
        public List<ShortCourseOnProgramme> OnProgramme { get; set; }
    }

    public class ShortCourseOnProgramme
    {
        [Required]
        public string? CourseCode { get; set; }
        public string? AgreementId { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? ExpectedEndDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        [Required]
        public List<LearningSupportRequestDetails> LearningSupport { get; set; }
        [Required]
        public DateTime? PauseDate { get; set; }
        [Required]
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
