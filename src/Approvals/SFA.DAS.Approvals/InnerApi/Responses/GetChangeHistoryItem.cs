using System;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetChangeHistoryItem
{
    public byte ChangeType { get; set; }
    public string Description { get; set; }
    public long ApprenticeshipId { get; set; }
    public string LearnerName { get; set; }
    public DateTime AppliedDate { get; set; }
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public string EmployerName { get; set; }
    public string ProviderName { get; set; }
}