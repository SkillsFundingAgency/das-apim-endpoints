using MediatR;
using System.Diagnostics.CodeAnalysis;
namespace SFA.DAS.Aodp.Application.Commands.Files
{
    [ExcludeFromCodeCoverage]
    public class DeleteFileMetadataCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {

        public Guid FileId { get; set; }
    }
}
