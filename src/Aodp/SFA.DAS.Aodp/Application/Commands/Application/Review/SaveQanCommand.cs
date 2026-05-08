using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveQanCommand : IRequest<BaseMediatrResponse<SaveQanCommandResponse>>
    {
        public Guid ApplicationReviewId { get; set; }

        [QualificationNumber]
        public string? Qan { get; set; }

        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string SentByName { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string SentByEmail { get; set; }
    }
}