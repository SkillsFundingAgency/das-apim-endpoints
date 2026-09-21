namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportCommittedLearners;

public sealed record ImportCommittedLearnersCommandResult
{
    public int TotalRecords { get; init; }
    public int FailedRecords { get; init; }
    public int BatchesProcessed { get; init; }
}