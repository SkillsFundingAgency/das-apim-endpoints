using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Files
{
    [ExcludeFromCodeCoverage]
    public class DeleteFileMetadataApiRequest : IDeleteApiRequest
    {
        public Guid FileId { get; set; }
        public string DeleteUrl => $"/api/files/{FileId}";
    }   
}
