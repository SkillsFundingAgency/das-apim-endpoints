namespace SFA.DAS.EmployerFinanceJobs.Api.Models.Requests;

public sealed record PostJobRequest
{
    public required long AccountId { get; set; }
    public required string JobName { get; set; }
    public required string JobState { get; set; }
    public required DateTime LastSuccessfulImportDate { get; set; }
    public required DateTime LastAttemptedDate { get; set; }
    public required bool LastAttemptSuccessful { get; set; }
    public required int TotalRecordsLastRun { get; set; }
    public required int FailedRecordsLastRun { get; set; }
}
