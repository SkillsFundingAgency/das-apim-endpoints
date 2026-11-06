using SFA.DAS.EmployerFinanceJobs.Domain.Enums;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record ApprenticeshipPaymentSummaryResponse
{
    public long ApprenticeshipId { get; set; }
    public long AccountId { get; set; }
    public bool IsTransfer { get; set; }
    public long Uln { get; set; }
    public DateOnly? Dob { get; set; }
    public ApprenticeshipStatus Status { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalPaid { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly? LastPaymentDate { get; set; }
    public decimal? LastPaymentAmount { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public DateTime? FreezedDate { get; set; }
}