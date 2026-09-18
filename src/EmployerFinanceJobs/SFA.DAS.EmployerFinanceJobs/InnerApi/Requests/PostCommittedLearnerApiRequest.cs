using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;


public sealed record PostCommittedLearnerApiRequest(long AccountId, PostCommittedLearnerApiRequestData Payload) : IPostApiRequest
{
    public string PostUrl => $"api/employer/{AccountId}/learners";
    public object Data { get; set; } = Payload;
}

public sealed record PostCommittedLearnerApiRequestData
{
    public long ApprenticeshipId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public required PaymentStatus PaymentStatus { get; set; }
}