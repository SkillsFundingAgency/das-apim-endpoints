using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateExitSurvey
{
    public class CreateExitSurveyCommandHandler : IRequestHandler<CreateExitSurveyCommand, CreateExitSurveyResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;

        public CreateExitSurveyCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<CreateExitSurveyResponse> Handle(CreateExitSurveyCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateExitSurveyRequest(new CreateExitSurveyData
            {
                ApprenticeFeedbackTargetId = command.ApprenticeFeedbackTargetId,
                DidNotCompleteApprenticeship = command.DidNotCompleteApprenticeship,
                IncompletionReason = command.IncompletionReason,
                IncompletionFactor_Caring = command.IncompletionFactor_Caring,
                IncompletionFactor_Family = command.IncompletionFactor_Family,
                IncompletionFactor_Financial = command.IncompletionFactor_Financial,
                IncompletionFactor_Mental = command.IncompletionFactor_Mental,
                IncompletionFactor_Physical = command.IncompletionFactor_Physical,
                IncompletionFactor_Other = command.IncompletionFactor_Other,
                RemainedReason = command.RemainedReason,
                ReasonForIncorrect = command.ReasonForIncorrect,
                AllowContact = command.AllowContact
            });

            var response = await _feedbackApiClient.PostWithResponseCode<CreateExitSurveyResponse>(request);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
