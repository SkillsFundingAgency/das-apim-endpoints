using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Query;

public class GetApprenticeshipApprovalResponse
{
    public long ApprenticeshipId { get; set; }
    public Guid ApprovalRequestId { get; set; }
    public int? ApprovalRequestStatus { get; set; }
    public virtual ICollection<ChangeItem> Items { get; set; }
    public string Name { get; set; }
    public string ULN { get; set; }
    public DateTime? StartDate { get; set; }
    public string CourseCode { get; set; }
    public string CourseName { get; set; }
    public string ProviderName { get; set; }
    public long UKPRN { get; set; }
    public string AccountLegalEntityName { get; set; }
    public long AccountLegalEntityId { get; set; }
    public long AccountId { get; set; }
    public bool ExceedsFundingCap { get; set; } = false;
    public int? FundingCap { get; set; }

    public class ChangeItem
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
    }
}