using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.TrackProgress.Apis.CoursesInnerApi;
using Microsoft.Extensions.Logging;
using System.Net;
using SFA.DAS.TrackProgress.Apis.TrackProgressInnerApi;
using SFA.DAS.TrackProgress.Application.DTOs;

namespace SFA.DAS.TrackProgress.Application.Services;

public class TrackProgressService
{
    private readonly IInternalApiClient<TrackProgressApiConfiguration> _trackProgressApi;
    private readonly ILogger<TrackProgressService> _logger;

    public TrackProgressService(IInternalApiClient<TrackProgressApiConfiguration> trackProgressApi, ILogger<TrackProgressService> logger)
    {
        _trackProgressApi = trackProgressApi;
        _logger = logger;
    }


    public async Task SaveProgress(long apprenticeshipId, KsbProgress data)
    {
        var request = new CreateTrackProgressRequest(apprenticeshipId, data);
        var result = await _trackProgressApi.PostWithResponseCode<GetKsbsForCourseOptionResponse>(request, false);

        if(result.StatusCode != HttpStatusCode.Created)
        {
            _logger.LogInformation("Unexpected response from Track Progress inner API when calling {0}",
                request.PostUrl);
            throw new TrackProgressApiException(result.StatusCode, result.ErrorContent);
        }
    }
}

public class TrackProgressApiException : Exception
{
    public TrackProgressApiException(HttpStatusCode statusCode, string details) : base(details)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}
