using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateOutcomeRecordedFeedbackTransaction
{
    public class CreateOutcomeRecordedFeedbackTransactionCommandHandler : IRequestHandler<CreateOutcomeRecordedFeedbackTransactionCommand, CreateOutcomeRecordedFeedbackTransactionResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apiClient;

        public CreateOutcomeRecordedFeedbackTransactionCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateOutcomeRecordedFeedbackTransactionResponse> Handle(CreateOutcomeRecordedFeedbackTransactionCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateOutcomeRecordedFeedbackTransactionRequest(
                new CreateOutcomeRecordedFeedbackTransactionData(command.ApprenticeshipId));

            var response = await _apiClient.PostWithResponseCode<object>(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new CreateOutcomeRecordedFeedbackTransactionResponse { NotFound = true };
            }

            response.EnsureSuccessStatusCode();
            return new CreateOutcomeRecordedFeedbackTransactionResponse();
        }
    }
}
