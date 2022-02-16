using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprentice
{
    public class CreateApprenticeCommandHandler : IRequestHandler<CreateApprenticeCommand>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;
       

        public CreateApprenticeCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<Unit> Handle(CreateApprenticeCommand command, CancellationToken cancellationToken)
        {
            // get commitments data

            //var commitment = await _commitmentsApiClient.GetWithResponseCode();

            var request = new PostAddApprenticeRequest(new AddApprenticeData
            {
                ApprenticeId = command.ApprenticeId,
                ApprenticeshipId = command.ApprenticeshipId,
                FirstName = command.FirstName,
                EmailAddress = command.EmailAddress,
                Status = command.Status
            });

            var result = await _feedbackApiClient.PostWithResponseCode<PostAddApprenticeRequest>(request);

            if (result.StatusCode != HttpStatusCode.Created)
            {
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);
            }

            return Unit.Value;
        }
    }
}
