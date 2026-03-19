using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

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
            var course = await GetCourseAsync(courseCode);
            var learnerAge = await GetLearnerAgeRulesAsync(course.LearningType);

            return new CourseTypeRulesResult
            {
                Course = course,
                LearnerAgeRules = learnerAge
            };
        }

        public async Task<RplRulesResult> GetRplRulesAsync(string courseCode)
        {
            var course = await GetCourseAsync(courseCode);
            var rplRules = await GetRplRulesInternalAsync(course.LearningType);

            return new RplRulesResult
            {
                Course = course,
                RplRules = rplRules
            };
        }

        private async Task<GetCourseLookupResponse> GetCourseAsync(string courseCode)
        {
            var course = await coursesApiClient.Get<GetCourseLookupResponse>(new GetCourseLookupByIdRequest(courseCode));
            
            if (course == null)
            {
                logger.LogError("Course not found for course ID {CourseId}", courseCode);
                throw new Exception($"Course not found for course ID {courseCode}");
            }

            return course;
        }

        private async Task<GetLearnerAgeResponse> GetLearnerAgeRulesAsync(string apprenticeshipType)
        {
            var request = new GetLearnerAgeRequest(apprenticeshipType);
            var response = await courseTypesApiClient.Get<GetLearnerAgeResponse>(request);

            if (response == null)
            {
                logger.LogError("Learner age rules not found for apprenticeship type {ApprenticeshipType}", apprenticeshipType);
                throw new Exception($"Learner age rules not found for apprenticeship type {apprenticeshipType}");
            }

            return response;
        }

        private async Task<GetRecognitionOfPriorLearningResponse> GetRplRulesInternalAsync(string apprenticeshipType)
        {
            var request = new GetRecognitionOfPriorLearningRequest(apprenticeshipType);
            var response = await courseTypesApiClient.Get<GetRecognitionOfPriorLearningResponse>(request);

            if (response == null)
            {
                logger.LogError("RPL rules not found for apprenticeship type {ApprenticeshipType}", apprenticeshipType);
                throw new Exception($"RPL rules not found for apprenticeship type {apprenticeshipType}");
            }

            return response;
        }
    }
} 