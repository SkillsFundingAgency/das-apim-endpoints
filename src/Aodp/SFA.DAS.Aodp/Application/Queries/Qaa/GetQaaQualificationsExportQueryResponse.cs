using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Queries.Qaa
{
    [ExcludeFromCodeCoverage]
    public class GetQaaQualificationsExportQueryResponse
    {
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "text/csv";
    }
}
