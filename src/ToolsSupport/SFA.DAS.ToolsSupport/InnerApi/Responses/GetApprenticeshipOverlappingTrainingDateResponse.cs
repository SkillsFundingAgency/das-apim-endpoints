namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class GetApprenticeshipOverlappingTrainingDateResponse
{
    public List<ApprenticeshipOverlappingTrainingDateRequest> OverlappingTrainingDateRequest { get; set; }
}

public class ApprenticeshipOverlappingTrainingDateRequest
{
    public long Id { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public long PreviousApprenticeshipId { get; set; }
    public OverlappingTrainingDateRequestResolutionType? ResolutionType { get; set; }
    public OverlappingTrainingDateRequestStatus Status { get; set; }
    public DateTime? ActionedOn { get; set; }
    public DateTime CreatedOn { get; set; }

}

public enum OverlappingTrainingDateRequestResolutionType : short
{
    CompletionDateEvent = 0,
    ApprenticeshipUpdate = 1,
    StopDateUpdate = 2,
    ApprenticeshipStopped = 3,
    DraftApprenticeshipUpdated = 4,
    DraftApprenticeshipDeleted = 5,
    ApprenticeshipIsStillActive = 6,
    ApprenticeshipEndDateUpdate = 7,
    ApprenticeshipStopDateIsCorrect = 8,
    ApprenticeshipEndDateIsCorrect = 9,
    CohortDeleted = 10,
}

public enum OverlappingTrainingDateRequestStatus : short
{
    Pending = 0,
    Resolved = 1,
    Rejected = 2
}