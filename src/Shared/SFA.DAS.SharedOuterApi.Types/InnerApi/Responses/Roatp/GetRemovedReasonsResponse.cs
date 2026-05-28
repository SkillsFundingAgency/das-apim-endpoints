namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

public class GetRemovedReasonsResponse
{
    public IEnumerable<RemovedReasonSummary> ReasonsForRemoval { get; set; } = Enumerable.Empty<RemovedReasonSummary>();
}