namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportLearnersByAccountId;

public sealed class ImportLearnersByAccountIdResult
{
    public bool IsSuccessful { get; set; }
    public int TotalRecordsProcessed { get; set; }
    public int FailedRecordsCount { get; set; }
    public DateTime AttemptedDate { get; set; }
    public DateTime? LastSuccessfulImportDate { get; set; }
    public string? FailureReason { get; set; }
}