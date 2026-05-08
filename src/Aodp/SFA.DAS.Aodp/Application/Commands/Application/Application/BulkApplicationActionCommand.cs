using MediatR;
using SFA.DAS.Aodp.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application
{
    [ExcludeFromCodeCoverage]
    public class BulkApplicationActionCommand : IRequest<BaseMediatrResponse<BulkApplicationActionCommandResponse>>
    {
        public List<Guid> ApplicationReviewIds { get; set; } = new();

        public BulkApplicationActionType? ActionType { get; set; }

        [UserType]
        public required string UserType { get; set; }
        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public required string SentByName { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public required string SentByEmail { get; set; }
    }
}