namespace SFA.DAS.EmployerFinanceJobs.Commands.UpdateLearnerCost;

public sealed record UpdateLearnerCostCommandResult
{
    public int TotalRecords { get; init; }
    public int SuccessfulRecords { get; init; }
    public int FailedRecords { get; init; }
}