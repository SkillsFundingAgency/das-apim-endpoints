using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction
{
    public class GenerateEmailTransactionCommandHandler : IRequestHandler<GenerateEmailTransactionCommand, NullResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;

        public GenerateEmailTransactionCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<NullResponse> Handle(GenerateEmailTransactionCommand command, CancellationToken cancellationToken)
        {
            var response = await _feedbackApiClient.PostWithResponseCode<NullResponse>(new GenerateEmailTransactionRequest(), false);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
