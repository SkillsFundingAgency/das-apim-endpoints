namespace SFA.DAS.Approvals.Api.Models.DataLock
{
    /// <summary>
    /// A subset of data relating to data locks for an apprenticeship
    /// </summary>
    public class DataLockStatusSummary
    {
        public long DataLockEventId { get; set; }
        public DataLockErrorCode ErrorCode { get; set; }
        public TriageStatus TriageStatus { get; set; }
    }
}