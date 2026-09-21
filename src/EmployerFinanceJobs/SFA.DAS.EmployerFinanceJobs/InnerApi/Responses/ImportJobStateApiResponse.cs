namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record ImportJobStateApiResponse
{
    public Guid Id { get; set; }
    public required string JobName { get; set; }
    public DateTime LastSuccessfulImportDate { get; set; }
    public DateTime LastAttemptedDate { get; set; }
    public bool LastAttemptSuccessful { get; set; }
    public int TotalRecordsLastRun { get; set; }
    public int FailedRecordsLastRun { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime CreatedDate { get; set; }
}