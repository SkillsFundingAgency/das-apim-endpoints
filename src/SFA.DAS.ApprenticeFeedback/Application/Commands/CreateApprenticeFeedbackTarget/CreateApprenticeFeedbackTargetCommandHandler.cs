using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedbackTarget
{
    public class CreateApprenticeFeedbackTargetCommandHandler : IRequestHandler<CreateApprenticeFeedbackTargetCommand, CreateApprenticeFeedbackTargetResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;

        public CreateApprenticeFeedbackTargetCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<CreateApprenticeFeedbackTargetResponse> Handle(CreateApprenticeFeedbackTargetCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateApprenticeFeedbackTargetRequest(new CreateApprenticeFeedbackTargetData
            {
                ApprenticeId = command.ApprenticeId,
                ApprenticeshipId = command.ApprenticeshipId,
                CommitmentApprenticeshipId = command.CommitmentsApprenticeshipId
            });

            var response = await _feedbackApiClient.PostWithResponseCode<object>(request);

            response.EnsureSuccessStatusCode();
            return new CreateApprenticeFeedbackTargetResponse();
        }
    }
}
