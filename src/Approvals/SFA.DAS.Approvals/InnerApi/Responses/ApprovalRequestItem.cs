using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class ApprovalRequestItem
{
    public Guid Id { get; set; }
    public Guid LearningKey { get; set; }
    public long ApprenticeshipId { get; set; }
    public byte LearningType { get; set; }
    public byte? Status { get; set; }
    public virtual ICollection<ApprovalFieldRequest> Items { get; set; }
    public bool? EmployerSeenAlert { get; set; }
}

public class ApprovalFieldRequest
{
    public string Field { get; set; }
    public string Old { get; set; }
    public string New { get; set; }
    public byte? Status { get; set; }
    public DateTime Created { get; set; }
}