using MediatR;
using SFA.DAS.Aodp.Validation;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveReviewOwnerUpdateCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public Guid ApplicationReviewId { get; set; }

        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string? Owner { get; set; }

        [UserType]
        public string UserType { get; set; }

        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string SentByName { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string SentByEmail { get; set; }
    }
}