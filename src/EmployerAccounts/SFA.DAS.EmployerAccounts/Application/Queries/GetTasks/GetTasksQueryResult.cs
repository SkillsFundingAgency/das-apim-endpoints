namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;

public class GetTasksQueryResult
{
    public bool ShowLevyDeclarationTask { get; set; }
    public int NumberOfApprenticesToReview { get; set; }
    public int NumberOfCohortsReadyToReview { get; set; }
    public int NumberOfPendingTransferConnections { get; set; }
    public int NumberOfTransferRequestToReview { get; set; }
    public int NumberTransferPledgeApplicationsToReview { get; set; }
    public int NumberOfTransferPledgeApplicationsAccepted  { get; set; }
    public int? SingleAcceptedTransferApplicationIdWithNoApprentices { get; set; } 
}