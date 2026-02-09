using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses
{
    public class CreateDraftShortCourseApiPostRequest(CreateDraftShortCourseRequest data) : IPostApiRequest
    {
        public string PostUrl { get; } = $"shortCourses";
        public object Data { get; set; } = data;
    }

    /// <summary>
    /// Request to create a draft short course learner record
    /// </summary>
    public class CreateDraftShortCourseRequest
    {
        /// <summary>
        /// Learner details to be updated
        /// </summary>
        public ShortCourseLearningUpdateDetails LearnerUpdateDetails { get; set; }

        /// <summary>
        /// Learning support details
        /// </summary>
        public List<LearningSupportUpdatedDetails> LearningSupport { get; set; } = new();

        /// <summary>
        /// On programme details
        /// </summary>
        public OnProgramme OnProgramme { get; set; }
    }

    public class ShortCourseLearningUpdateDetails : LearningUpdateDetails
    {
        public long Uln { get; set; }
    }

    /// <summary>
    /// On programme details
    /// </summary>
    public class OnProgramme
    {
        /// <summary>
        /// Course code
        /// </summary>
        public string CourseCode { get; set; } = null!;

        /// <summary>
        /// Employer identifier
        /// </summary>
        public long EmployerId { get; set; }

        /// <summary>
        /// Provider UKPRN
        /// </summary>
        public long Ukprn { get; set; }

        /// <summary>
        /// Start date of the short course
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Withdrawal date of the short course
        /// </summary>
        public DateTime? WithdrawalDate { get; set; }

        /// <summary>
        /// Completion date of the short course
        /// </summary>
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        /// Expected end date of the short course
        /// </summary>
        public DateTime ExpectedEndDate { get; set; }

        /// <summary>
        /// Milestones for the short course
        /// </summary>
        public List<Milestone> Milestones { get; set; } = new();

        /// <summary>
        /// Price of the short course
        /// </summary>
        public decimal Price { get; set; }
    }

    public enum Milestone
    {
        ThirtyPercentLearningComplete,
        LearningComplete
    }
}
