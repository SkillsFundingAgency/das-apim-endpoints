using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgressInternal.Apis.TrackProgressInnerApi;

public record AggregateProgressRequest(long CommitmentsApprenticeshipId) : IPostApiRequest
{
    public string PostUrl => $"/apprenticeships/{CommitmentsApprenticeshipId}/aggregate";

    public object Data { get; set; } = new { };
}