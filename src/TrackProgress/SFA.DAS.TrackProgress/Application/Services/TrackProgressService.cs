using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.TrackProgress.Apis.TrackProgressInnerApi;
using System.Net;
using SFA.DAS.TrackProgress.Application.Commands.TrackProgress;

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

    public async Task<TrackProgressResponse> SaveProgress(KsbProgress data)
    {
        var request = new CreateTrackProgressRequest(data);
        var result = await _trackProgressApi.PostWithResponseCode<TrackProgressResponse>(request);

        if (result.StatusCode != HttpStatusCode.Created)
        {
            _logger.LogInformation("Unexpected response from Track Progress inner API when calling {Url}",
                request.PostUrl);
            throw new TrackProgressApiException(result.StatusCode, result.ErrorContent);
        }

        return result.Body;
    }
}