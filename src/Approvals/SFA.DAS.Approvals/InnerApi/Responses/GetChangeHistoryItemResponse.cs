using System;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetChangeHistoryItemResponse
{
    public Guid Id { get; set; }
    public byte Source { get; set; }
    public byte ChangeType { get; set; }
    public string Description { get; set; }
    public Guid? UserId { get; set; }
    public long ApprenticeshipId { get; set; }
    public string LearnerName { get; set; }
    public Guid? LearningKey { get; set; }
    public DateTime Created { get; set; }
    public DateTime AppliedDate { get; set; }
    public long AccountId { get; set; }
    public long UKPRN { get; set; }
    public string ProviderName { get; set; }
    public string EmployerName { get; set; }
}