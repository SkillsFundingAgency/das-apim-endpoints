namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportCommittedLearners;

public sealed record ImportCommittedLearnersCommandResult
{
    public int TotalRecords { get; set; }
    public int FailedRecords { get; set; }
    public int BatchesProcessed { get; set; }
}