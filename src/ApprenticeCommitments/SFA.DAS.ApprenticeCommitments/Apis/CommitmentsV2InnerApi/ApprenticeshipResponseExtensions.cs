using Microsoft.Extensions.Logging;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi
{
    public static class ApprenticeshipResponseExtensions
    {
        public static string? GetCourseCode(this ApprenticeshipResponse apprenticeship, ILogger logger)
        {
            if (apprenticeship.CourseCode.Contains("-"))
            {
                logger.LogWarning("Apprenticeship {apprenticeshipId} is for a framework, no point in calling Apprentice Commitments", apprenticeship.Id);
                return default;
            }

            return ApprenticeCourseCode(apprenticeship);
        }

        private static string ApprenticeCourseCode(ApprenticeshipResponse apprenticeship)
        {
            // Remove after Standards Versioning goes live
            // Revert to just StandardUId
            return string.IsNullOrWhiteSpace(apprenticeship.StandardUId)
                ? apprenticeship.CourseCode : apprenticeship.StandardUId;
        }
    }
}