using MediatR;
using SFA.DAS.Aodp.Application.Queries.Files;
using System.Diagnostics.CodeAnalysis;
namespace SFA.DAS.Aodp.Application.Commands.Files
{
    [ExcludeFromCodeCoverage]
    public class CreateFileMetadataCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public FileCategory FileCategory { get; init; }
        public Guid? ApplicationId { get; init; }
        public Guid? MessageId { get; init; }
        public Guid? QuestionId { get; init; }
        public string FileName { get; init; } = string.Empty;
        public string ContentType { get; init; } = string.Empty;
        public string BlobContainer { get; init; } = string.Empty;
        public string BlobPath { get; init; } = string.Empty;
        public string UploadedBy { get; init; } = string.Empty;



    }
}
