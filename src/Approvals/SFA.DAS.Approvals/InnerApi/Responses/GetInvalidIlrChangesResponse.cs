using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetInvalidIlrChangesResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<InvalidIlrChangeSet> RequestSets { get; set; } = [];
}

public class InvalidIlrChangeSet
{
    public Guid ApprovalRequestId { get; set; }
    public string Decision { get; set; }
    public List<InvalidIlrChangeField> Fields { get; set; } = [];
}

public class InvalidIlrChangeField
{
    public string Field { get; set; }
    public string Old { get; set; }
    public string New { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public string Reason { get; set; }
}
