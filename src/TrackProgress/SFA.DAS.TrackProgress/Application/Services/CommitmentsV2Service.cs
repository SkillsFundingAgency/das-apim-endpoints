using System.Net;
using Microsoft.Extensions.Logging;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgress.Application.Services;

public class CommitmentsV2Service
{
    private readonly IInternalApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2Api;
    private readonly ILogger<CommitmentsV2Service> _logger;

    public CommitmentsV2Service(IInternalApiClient<CommitmentsV2ApiConfiguration> commitmentsV2Api, ILogger<CommitmentsV2Service> logger)
    {
        _commitmentsV2Api = commitmentsV2Api;
        _logger = logger;
    }

    public async Task<GetApprenticeshipsResponse> GetApprenticeships(long providerId, long uln, DateTime startDate)
    {
        var request = new GetApprenticeshipsRequest(providerId, uln, startDate);
        var result = await _commitmentsV2Api.GetWithResponseCode<GetApprenticeshipsResponse>(request);

        if (result.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogInformation("Unexpected response from Commitments V2 API when calling {0}", request.GetUrl);
            throw new CommitmentsApiException(result.StatusCode, result.ErrorContent);
        }

        return result.Body;
    }

    public async Task<GetApprenticeshipResponse> GetApprenticeship(long apprenticeshipId)
    {
        var request = new GetApprenticeshipRequest(apprenticeshipId);
        var result = await _commitmentsV2Api.GetWithResponseCode<GetApprenticeshipResponse>(request);

        if (result.Body == null)
        {
            _logger.LogInformation("No apprenticeship found in Commitments V2 API when calling {0}", request.GetUrl);
            throw new CommitmentsApiException(result.StatusCode, result.ErrorContent);
        }

        return result.Body;
    }
}

public class CommitmentsApiException : Exception
{
    public CommitmentsApiException(HttpStatusCode statusCode, string details) : base(details)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}
