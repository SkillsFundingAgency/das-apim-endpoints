using MediatR;
using System.Diagnostics.CodeAnalysis;
namespace SFA.DAS.Aodp.Application.Queries.Files
{
    [ExcludeFromCodeCoverage]
    public class GetFileMetadataQuery : IRequest<BaseMediatrResponse<GetFileMetadataQueryResponse>>
    {
        public IEnumerable<FileCategory>? FileCategories { get; init; }
        public Guid? FileId { get; init; }
        public Guid? ApplicationId { get; init; }
        public Guid? MessageId { get; init; }
        public Guid? QuestionId { get; init; }
    }
}
