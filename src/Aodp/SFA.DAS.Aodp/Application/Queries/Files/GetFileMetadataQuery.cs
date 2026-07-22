using MediatR;
using SFA.DAS.Aodp.Application.Queries.Files;
using System.Diagnostics.CodeAnalysis;
namespace SFA.DAS.Aodp.Application.Queries.Files
{
    [ExcludeFromCodeCoverage]
    public class GetFileMetadataQuery : IRequest<BaseMediatrResponse<GetFileMetadataQueryResponse>>
    {
        public IEnumerable<FileCategory>? FileCategories { get; init; }
        public Guid? FileId { get; set; }
        public Guid? ApplicationId { get; init; }
        public Guid? MessageId { get; init; }
        public Guid? QuestionId { get; init; }
    }
}
