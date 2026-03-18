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

namespace SFA.DAS.ApprenticePortal.Services
{
    public class CoursesService
    {
        private readonly CourseApiClient _client;

        public CoursesService(CourseApiClient client) => _client = client;

        public async Task<StandardApiResponse> GetStandardCourse(string standardUId)
        {
            var response = await _client.GetWithResponseCode<StandardApiResponse>(
                new GetStandardDetailsByIdRequest(standardUId));

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestException(response.ErrorContent);

            return response.Body;
        }

        public async Task<FrameworkApiResponse> GetFrameworkCourse(string frameworkCode)
        {
            var response = await _client.GetWithResponseCode<FrameworkApiResponse>(
                new GetFrameworkRequest(frameworkCode));

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestException(response.ErrorContent);

            return response.Body;
        }
    }

    public class FrameworkApiResponse
    {
        public string Title { get; set; }
    }

    public class StandardApiResponse : StandardApiResponseBase
    {
        public string Title { get; set; }
    }

}