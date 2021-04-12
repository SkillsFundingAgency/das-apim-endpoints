using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class CoursesService
    {
        private readonly CourseApiClient _client;

        public CoursesService(CourseApiClient client) => _client = client;

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client);

        public async Task<StandardApiResponse> GetCourse(string courseCode)
        {
            var course = await _client.Get<StandardApiResponse>(
                new GetStandardDetailsByIdRequest(courseCode));

            if (course == null)
            {
                throw new HttpRequestException($"Course `{courseCode}` not found");
            }

            return course;
        }
    }

    public class StandardApiResponse : StandardApiResponseBase
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public string Option { get; set; }
    }
}