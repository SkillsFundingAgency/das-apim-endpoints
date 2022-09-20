namespace SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;

public class GetApprenticeshipResponse
{
    public long Id { get; set; }
    public string StandardUId { get; set; } = null!;
    public string? Option { get; set; }
    public string Uln { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? StopDate { get; set; }
    public DeliveryModel DeliveryModel { get; set; }
    public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
    public long? ContinuationOfId { get; set; }
}
