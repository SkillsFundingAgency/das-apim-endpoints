using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Queries.Qaa
{
    [ExcludeFromCodeCoverage]
    public class GetQaaDownloadSummaryQueryResponse
    {
        public DateTime? DataLastImportedDate { get; set; }
        public DateTime? MostRecentDownloadDate { get; set; }
        public int NewQualificationsCount { get; set; }
        public int ExtendedQualificationsCount { get; set; }
        public int DiscontinuedQualificationsCount { get; set; }
        public List<QaaDownloadLog> DownloadHistory { get; set; } = new();

        public class QaaDownloadLog
        {
            public string? UserDisplayName { get; set; }
            public DateTime DownloadDate { get; set; }
        }
    }
}
