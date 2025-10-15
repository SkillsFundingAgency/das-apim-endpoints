namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationOutputFileResponse
    {
        public byte[] ZipFileContent { get; init; }
        public string FileName { get; init; }         
        public string ContentType { get; init; } = "application/zip";
    }

}
