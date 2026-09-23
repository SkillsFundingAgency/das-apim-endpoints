namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record RecalculateFundingProjectionApiResponse
{
    public long TotalRecordsProcessed { get; init; }
    public long TotalRecordsUpdated { get; init; }
    public long TotalRecordsInserted { get; init; }
}