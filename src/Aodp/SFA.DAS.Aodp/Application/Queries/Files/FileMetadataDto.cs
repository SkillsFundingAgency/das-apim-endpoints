using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Queries.Files
{
    [ExcludeFromCodeCoverage]
    public class FileMetadataDto
    {
        public Guid FileId { get; init; }
        public string FileName { get; init; } = string.Empty;
        public FileCategory FileCategory { get; set; } = FileCategory.Unknown;
        public string ContentType { get; set; } = string.Empty;
        public string BlobContainer { get; init; } = string.Empty;
        public string BlobPath { get; set; } = string.Empty;
        public Guid? ApplicationId { get; init; }
        public Guid? MessageId { get; init; }
        public Guid? QuestionId { get; init; }
        public bool IsDownloadable { get; init; }
    }
}
