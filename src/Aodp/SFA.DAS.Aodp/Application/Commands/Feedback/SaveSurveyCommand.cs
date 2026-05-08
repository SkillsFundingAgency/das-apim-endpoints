using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.Feedback
{
    public class SaveSurveyCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string Page { get; set; }

        public int SatisfactionScore { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string Comments { get; set; }
    }
}