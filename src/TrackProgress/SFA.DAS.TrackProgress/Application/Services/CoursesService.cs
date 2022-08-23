using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.TrackProgress.Apis.CoursesInnerApi;
using Microsoft.Extensions.Logging;
using System.Net;

namespace SFA.DAS.TrackProgress.Application.Services;

public class CoursesService
{
    private readonly IInternalApiClient<CoursesApiConfiguration> _coursesApi;
    private readonly ILogger<CoursesService> _logger;

    public CoursesService(IInternalApiClient<CoursesApiConfiguration> coursesApi, ILogger<CoursesService> logger)
    { 
        _coursesApi = coursesApi;
        _logger = logger;
    }

    public async Task<GetCourseOptionsResponse> GetOptionsForCourse(string standardUId)
    {
        var request = new GetCourseOptionsRequest(standardUId);
        var result = await _coursesApi.GetWithResponseCode<GetCourseOptionsResponse>(request);

        if (result.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogInformation("Unexpected response from Courses API when calling {0}", request.GetUrl);
            throw new CourseApiException(result.StatusCode, result.ErrorContent);
        }

        return result.Body;
    }

    public async Task<GetKsbsForCourseOptionResponse> GetKsbsForCourseOption(string standardUId, string option)
    {
        var request = new GetKsbsForCourseOptionRequest(standardUId, option);
        var result = await _coursesApi.GetWithResponseCode<GetKsbsForCourseOptionResponse>(request);

        if(result.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogInformation("Unexpected response from Courses API when calling {0}", request.GetUrl);
            throw new CourseApiException(result.StatusCode, result.ErrorContent);
        }

        return result.Body;
    }
}

public class CourseApiException : Exception
{
    public CourseApiException(HttpStatusCode statusCode, string details) : base(details)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}
