namespace SFA.DAS.EmployerFinanceJobs.Commands.RecalculateFundingProjection;

public sealed record RecalculateFundingProjectionCommandResult(long TotalRecordsProcessed,
    long TotalRecordsUpdated,
    long TotalRecordsInserted);