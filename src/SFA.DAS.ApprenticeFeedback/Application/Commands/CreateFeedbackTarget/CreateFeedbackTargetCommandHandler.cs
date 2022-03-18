using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateFeedbackTarget
{
    public class CreateFeedbackTargetCommandHandler : IRequestHandler<CreateFeedbackTargetCommand, CreateFeedbackTargetResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;

        public CreateFeedbackTargetCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<CreateFeedbackTargetResponse> Handle(CreateFeedbackTargetCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateFeedbackTargetRequest(new CreateFeedbackTargetData
            {
                ApprenticeId = command.ApprenticeId,
                ApprenticeshipId = command.ApprenticeshipId,
                CommitmentApprenticeshipId = command.CommitmentsApprenticeshipId
            });

            var response = await _feedbackApiClient.PostWithResponseCode<object>(request);

            response.EnsureSuccessStatusCode();
            return new CreateFeedbackTargetResponse();
        }
    }
}
