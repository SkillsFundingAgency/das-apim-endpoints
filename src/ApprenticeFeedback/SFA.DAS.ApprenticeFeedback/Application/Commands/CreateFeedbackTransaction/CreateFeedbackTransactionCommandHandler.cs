using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateFeedbackTransaction
{
    public class CreateFeedbackTransactionCommandHandler : IRequestHandler<CreateFeedbackTransactionCommand, CreateFeedbackTransactionResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apiClient;

        public CreateFeedbackTransactionCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateFeedbackTransactionResponse> Handle(CreateFeedbackTransactionCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateFeedbackTransactionRequest(
                new CreateFeedbackTransactionData(command.ApprenticeshipId, command.TemplateName));

            var response = await _apiClient.PostWithResponseCode<object>(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new CreateFeedbackTransactionResponse { NotFound = true };
            }

            response.EnsureSuccessStatusCode();
            return new CreateFeedbackTransactionResponse();
        }
    }
}
