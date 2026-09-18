using SFA.DAS.EmployerFinanceJobs.Domain.Enums;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record GetCommittedLearnerApiResponse
{
    public Guid Id { get; set; }
    public long ApprenticeshipId { get; set; }
    public long? CommitmentId { get; set; }
    public decimal Cost { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime ImportedOn { get; set; }
    public required PaymentStatus PaymentStatus { get; set; }
}