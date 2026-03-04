namespace SFA.DAS.LearnerData.Application.GetShortCourseEarnings
{
    public class GetShortCourseEarningsResponse
    {
        public ShortCourseLearnerAndEarnings[] Learners { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    public class ShortCourseLearnerAndEarnings
    {
        public Guid LearningKey { get; set; }
        public string LearnerRef { get; set; }
        public ShortCourseCourse Course { get; set; }
    }

    public class ShortCourseCourse
    {
        public int AimSequenceNumber { get; set; }
        public string FundingLineType { get; set; }
        public decimal CoursePrice { get; set; }
        public ShortCourseEarning[] Earnings { get; set; }
    }

    public class ShortCourseEarning
    {
        public int CollectionYear { get; set; }
        public int CollectionMonth { get; set; }
        public ShortCourseMilestone Milestone { get; set; }
        public decimal Amount { get; set; }
        public bool Approved { get; set; }
    }

    public enum ShortCourseMilestone
    {
        ThirtyPercentLearningComplete,
        LearningComplete
    }
}
