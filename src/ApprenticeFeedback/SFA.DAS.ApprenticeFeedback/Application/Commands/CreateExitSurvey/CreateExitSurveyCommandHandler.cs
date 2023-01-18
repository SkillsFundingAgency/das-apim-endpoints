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
                AllowContact = command.AllowContact,
                AttributeIds = command.AttributeIds,
                PrimaryReason = command.PrimaryReason
            });

            var response = await _feedbackApiClient.PostWithResponseCode<CreateExitSurveyResponse>(request);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
