using MediatR;
using SFA.DAS.Aodp.Validation;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveReviewerCommand : IRequest<BaseMediatrResponse<SaveReviewerCommandResponse>>
    {
        public Guid ApplicationId { get; set; }
        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string ReviewerFieldName { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string? ReviewerValue { get; set; }

        [UserType]
        public string UserType { get; set; }

        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string SentByName { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string SentByEmail { get; set; }
    }
}