namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetDiscussionHistoriesForQualificationQueryResponse
{
    public List<QualificationDiscussionHistory> QualificationDiscussionHistories { get; set; } = new List<QualificationDiscussionHistory>();
    public partial class QualificationDiscussionHistory
    {
        public Guid Id { get; set; }
        public Guid QualificationId { get; set; }
        public Guid ActionTypeId { get; set; }
        public string? UserDisplayName { get; set; }
        public string? Notes { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Title { get; set; }
        public virtual ActionType ActionType { get; set; } = null!;
    }
    public class ActionType
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
    }


}
