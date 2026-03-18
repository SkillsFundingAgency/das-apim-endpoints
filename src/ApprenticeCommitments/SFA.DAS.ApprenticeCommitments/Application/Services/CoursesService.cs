using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.SharedOuterApi.Types.Services;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class CoursesService
    {
        private readonly CourseApiClient _client;

        public CoursesService(CourseApiClient client) => _client = client;

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client);

        public async Task<StandardApiResponse> GetCourse(string courseCode)
        {
            var course = await _client.GetWithResponseCode<StandardApiResponse>(
                new GetStandardDetailsByIdRequest(courseCode));

            if (course.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestException(course.ErrorContent);

            return course.Body;
        }
    }

    public class StandardApiResponse : StandardApiResponseBase
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public string Option { get; set; }
        public string ApprenticeshipType { get; set; }
    }
}