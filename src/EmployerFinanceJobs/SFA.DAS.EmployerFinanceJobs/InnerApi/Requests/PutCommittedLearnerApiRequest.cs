using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record PutCommittedLearnerApiRequest(long AccountId, Guid Id, PutCommittedLearnerApiRequestData Payload) : IPutApiRequest
{
    public string PutUrl => $"api/employer/{AccountId}/learners/{Id}";
    public object Data { get; set; } = Payload;
}

public sealed record PutCommittedLearnerApiRequestData
{
    public long ApprenticeshipId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public required PaymentStatus PaymentStatus { get; set; }
    public long? CommitmentId { get; set; }
    public decimal Cost { get; set; }
}