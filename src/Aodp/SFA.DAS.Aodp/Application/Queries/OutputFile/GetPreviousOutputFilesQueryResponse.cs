using System.Globalization;

namespace SFA.DAS.AODP.Application.Queries.OutputFile;

public class GetPreviousOutputFilesQueryResponse
{
    public List<File> GeneratedFiles { get; set; } = new List<File>();
    public class File
    {
        public string DisplayName { get; set; } = string.Empty;
        public string? BlobName { get; set; }
        public bool IsInProgress { get; set; }
        public DateTime? Created { get; set; }
    }
}