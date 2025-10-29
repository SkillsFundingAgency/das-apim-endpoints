namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationOutputFileLogResponse
    {
        public IEnumerable<QualificationOutputFileLog> OutputFileLogs { get; set; } = new List<QualificationOutputFileLog>();

        public partial class QualificationOutputFileLog
        {
            public Guid Id { get; set; }
            public string? UserDisplayName { get; set; }
            public DateTime? Timestamp { get; set; }
            public string? ApprovedFileName { get; set; }
            public string? ArchivedFileName { get; set; }
        }
    }
}
