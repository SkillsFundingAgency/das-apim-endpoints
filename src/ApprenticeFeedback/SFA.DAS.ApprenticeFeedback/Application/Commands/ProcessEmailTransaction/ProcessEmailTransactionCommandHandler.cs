
using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;


namespace SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction
{
    public class ProcessEmailTransactionCommandHandler : IRequestHandler<ProcessEmailTransactionCommand, ProcessEmailTransactionResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apiClient;

        public ProcessEmailTransactionCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ProcessEmailTransactionResponse> Handle(ProcessEmailTransactionCommand command, CancellationToken cancellationToken)
        {
            var request = new ProcessEmailTransactionRequest(new ProcessEmailTransactionData(
                command.FeedbackTransactionId,
                command.ApprenticeName,
                command.ApprenticeEmailAddress,
                command.IsEmailContactAllowed
            ));

            var response = await _apiClient.PostWithResponseCode<ProcessEmailTransactionResponse>(request);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
