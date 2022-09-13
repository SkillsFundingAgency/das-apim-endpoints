using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;

public class GetApprenticeshipRequest : IGetApiRequest
{
    private readonly long _apprenticeshipId;

    public GetApprenticeshipRequest(long apprenticeshipId)
        => _apprenticeshipId = apprenticeshipId;

    public string GetUrl => $"api/apprenticeships/{_apprenticeshipId}";
}