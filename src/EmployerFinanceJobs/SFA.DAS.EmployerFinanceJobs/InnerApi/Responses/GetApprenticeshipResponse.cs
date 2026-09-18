namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record GetApprenticeshipResponse
{
    public long Id { get; set; }
    public long CohortId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public long EmployerAccountId { get; set; }
    public int? PriceReducedBy { get; set; }
}