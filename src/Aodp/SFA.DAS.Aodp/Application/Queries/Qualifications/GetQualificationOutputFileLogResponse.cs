namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationOutputFileLogResponse
    {
        public IEnumerable<QualificationOutputFileLog> OutputFileLogs { get; set; } = new List<QualificationOutputFileLog>();

        public partial class QualificationOutputFileLog
        {
            public Guid Id { get; set; }
            public string? UserDisplayName { get; set; }
            public DateTime? DownloadDate { get; set; }
            public DateTime? PublicationDate { get; set; }
            public string? FileName { get; set; }
        }
    }
}
