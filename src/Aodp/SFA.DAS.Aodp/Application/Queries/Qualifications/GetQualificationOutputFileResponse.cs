namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationOutputFileResponse
    {
        public byte[] ZipFileContent { get; init; } = Array.Empty<byte>();
        public string FileName { get; init; } = string.Empty;       
        public string ContentType { get; init; } = "application/zip";
    }

}
