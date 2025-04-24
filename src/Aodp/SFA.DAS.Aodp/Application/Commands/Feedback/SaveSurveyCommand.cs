using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Feedback
{
    public class SaveSurveyCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public string Page { get; set; }

        public int SatisfactionScore { get; set; }

        public string Comments { get; set; }
    }
}