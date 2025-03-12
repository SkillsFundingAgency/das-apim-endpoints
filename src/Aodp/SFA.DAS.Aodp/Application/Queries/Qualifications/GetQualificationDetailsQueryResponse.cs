using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetQualificationDetailsQueryResponse
{
    public int Id { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? Changes { get; set; }
    public string? QualificationReference { get; set; }
    public string? AwardingOrganisation { get; set; }
    public string? Title { get; set; }
    public string? QualificationType { get; set; }
    public string? Level { get; set; }
    public string? ProposedChanges { get; set; }
    public string? AgeGroup { get; set; }
    public string? Category { get; set; }
    public string? Subject { get; set; }
    public string? SectorSubjectArea { get; set; }
    public string? Comments { get; set; }
    public List<QualificationDiscussionHistory> QualificationDiscussionHistories { get; set; }
}

public class QualificationDiscussionHistory
{
    public Guid Id { get; set; }
    public Guid QualificationId { get; set; }
    public Guid ActionTypeId { get; set; }
    public string? UserDisplayName { get; set; }
    public string? Notes { get; set; }
    public DateTime? Timestamp { get; set; }
    public virtual ActionType ActionType { get; set; } = null!;
}

public class ActionType
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
}
