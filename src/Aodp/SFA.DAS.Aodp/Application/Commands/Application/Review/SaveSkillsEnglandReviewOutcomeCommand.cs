using MediatR;
using SFA.DAS.Aodp.Validation;
namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveSkillsEnglandReviewOutcomeCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public Guid ApplicationReviewId { get; set; }
        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string? Comments { get; set; }
        public bool Approved { get; set; }
        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string SentByName { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string SentByEmail { get; set; }
    }
}