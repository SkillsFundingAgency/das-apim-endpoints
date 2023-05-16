using SFA.DAS.Approvals.Application.Apprentices.Queries.CheckReviewApprenticeshipCourse;

namespace SFA.DAS.Approvals.Api.Models.Apprentices
{
    public class CheckReviewApprenticeshipCourseResponse
    {
        public bool IsValidCourseCode { get; set; }

        public static implicit operator CheckReviewApprenticeshipCourseResponse(CheckReviewApprenticeshipCourseQueryResult source)
        {
            return new CheckReviewApprenticeshipCourseResponse
            {
                IsValidCourseCode = source.IsValidCourseCode
            };
        }
    }
}
