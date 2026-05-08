using MediatR;
using SFA.DAS.Aodp.Validation;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class UpdateApplicationReviewSharingCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        [UserType]
        public string ApplicationReviewUserType { get; set; }

        public bool ShareApplication { get; set; }
        public Guid ApplicationReviewId { get; set; }

        [UserType]
        public string UserType { get; set; }

        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string SentByName { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string SentByEmail { get; set; }
    }

}
