using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services
{
    public class CourseTypeRulesService(
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ICourseTypesApiClient courseTypesApiClient,
        ILogger<CourseTypeRulesService> logger)
        : ICourseTypeRulesService
    {
        public async Task<CourseTypeRulesResult> GetCourseTypeRulesAsync(string courseCode)
        {
            var standard = await coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(courseCode));
            
            if (standard == null)
            {
                logger.LogError("Standard not found for course ID {CourseId}", courseCode);
                throw new Exception($"Standard not found for course ID {courseCode}");
            }
            
            var learnerAge = await courseTypesApiClient.Get<GetLearnerAgeResponse>(new GetLearnerAgeRequest(standard.ApprenticeshipType));

            if (learnerAge == null)
            {
                logger.LogError("Learner age rules not found for apprenticeship type {ApprenticeshipType}", standard.ApprenticeshipType);
                throw new Exception($"Learner age rules not found for apprenticeship type {standard.ApprenticeshipType}");
            }

            return new CourseTypeRulesResult
            {
                Standard = standard,
                LearnerAgeRules = learnerAge
            };
        }

        public async Task<RplRulesResult> GetRplRulesAsync(string courseCode)
        {
            var standard = await coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(courseCode));
            
            if (standard == null)
            {
                logger.LogError("Standard not found for course ID {CourseId}", courseCode);
                throw new Exception($"Standard not found for course ID {courseCode}");
            }
            
            var rplRules = await courseTypesApiClient.Get<GetRecognitionOfPriorLearningResponse>(new GetRecognitionOfPriorLearningRequest(standard.ApprenticeshipType));

            if (rplRules == null)
            {
                logger.LogError("RPL rules not found for apprenticeship type {ApprenticeshipType}", standard.ApprenticeshipType);
                throw new Exception($"RPL rules not found for apprenticeship type {standard.ApprenticeshipType}");
            }

            return new RplRulesResult
            {
                Standard = standard,
                RplRules = rplRules
            };
        }
    }
} 