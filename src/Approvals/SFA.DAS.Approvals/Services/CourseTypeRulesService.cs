using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
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
            logger.LogInformation("Getting old course type rules for course code {CourseCode}", courseCode);
            var standard = await GetStandardAsync(courseCode);
            var learnerAge = await GetLearnerAgeRulesAsync(standard.LearningType);

            return new CourseTypeRulesResult
            {
                Course = standard,
                LearnerAgeRules = learnerAge
            };
        }

        public async Task<RplRulesResult> GetRplRulesAsync(string courseCode)
        {
            var standard = await GetStandardAsync(courseCode);
            var rplRules = await GetRplRulesInternalAsync(standard.LearningType);

            return new RplRulesResult
            {
                Course = standard,
                RplRules = rplRules
            };
        }

        private async Task<GetCourseLookupResponse> GetStandardAsync(string courseCode)
        {
            var standard = await coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(courseCode));

            if (standard == null)
            {
                logger.LogError("Standard not found for course ID {CourseId}", courseCode);
                throw new Exception($"Standard not found for course ID {courseCode}");
            }

            return MapFromStandardResponseToCourseResponse(standard);
        }

        private static GetCourseLookupResponse MapFromStandardResponseToCourseResponse(GetStandardsListItem standardResponse)
        {
            return new GetCourseLookupResponse
            {
                StandardUId = standardResponse.StandardUId,
                IfateReferenceNumber = standardResponse.IfateReferenceNumber,
                LarsCode = standardResponse.LarsCode.ToString(),
                Status = standardResponse.Status,
                Title = standardResponse.Title,
                Options = standardResponse.Options,
                Level = standardResponse.Level,
                Version = standardResponse.Version,
                VersionMajor = standardResponse.VersionMajor,
                VersionMinor = standardResponse.VersionMinor,
                VersionDetail = standardResponse.VersionDetail,
                StandardPageUrl = standardResponse.StandardPageUrl,
                Route = standardResponse.Route,
                LearningType = standardResponse.ApprenticeshipType
            };
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