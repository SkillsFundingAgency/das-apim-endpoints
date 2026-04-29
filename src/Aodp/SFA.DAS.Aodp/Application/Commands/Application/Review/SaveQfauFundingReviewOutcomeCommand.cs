using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveQfauFundingReviewOutcomeCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public Guid ApplicationReviewId { get; set; }
        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string? Comments { get; set; }
        public bool Approved { get; set; }
    }

}