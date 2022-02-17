using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprentice
{
    public class CreateApprenticeCommandHandler : IRequestHandler<CreateApprenticeCommand>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;

        public CreateApprenticeCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<Unit> Handle(CreateApprenticeCommand command, CancellationToken cancellationToken)
        {
            var request = new PostCreateApprenticeRequest(new AddApprenticeData
            {
                ApprenticeId = command.ApprenticeId,
                ApprenticeshipId = command.ApprenticeshipId,
                Status = null
            });

            var response = await _feedbackApiClient.PostWithResponseCode<object>(request);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
