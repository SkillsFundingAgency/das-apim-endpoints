namespace SFA.DAS.EmployerFinanceJobs.Commands.UpdateLearnerCost;

public sealed record UpdateLearnerCostCommandResult
{
    public int TotalRecords { get; set; }
    public int SuccessfulRecords { get; set; }
    public int FailedRecords { get; set; }
}