namespace SFA.DAS.Aodp.Application.Queries.Rollover
{
    public class GetRolloverCandidatesForExportQueryResponse
    {
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "text/csv";
    }
}