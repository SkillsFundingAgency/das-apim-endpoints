using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application
{
    [ExcludeFromCodeCoverage]
    public class BulkApplicationActionCommand : IRequest<BaseMediatrResponse<BulkApplicationActionCommandResponse>>
    {
        public List<Guid> ApplicationReviewIds { get; set; } = new();

        public BulkApplicationActionType? ActionType { get; set; }

        public required string UserType { get; set; }
        public required string SentByName { get; set; }
        public required string SentByEmail { get; set; }
    }
}