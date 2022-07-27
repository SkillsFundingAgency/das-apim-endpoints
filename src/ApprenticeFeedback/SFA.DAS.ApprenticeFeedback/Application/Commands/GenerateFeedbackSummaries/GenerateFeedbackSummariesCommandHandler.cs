using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateFeedbackSummaries
{
    public class GenerateFeedbackSummariesCommandHandler : IRequestHandler<GenerateFeedbackSummariesCommand, GenerateFeedbackSummariesResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;

        public GenerateFeedbackSummariesCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<GenerateFeedbackSummariesResponse> Handle(GenerateFeedbackSummariesCommand command, CancellationToken cancellationToken)
        {
            var response = await _feedbackApiClient.PostWithResponseCode<object>(new GenerateFeedbackSummariesRequest());

            response.EnsureSuccessStatusCode();
            return new GenerateFeedbackSummariesResponse();
        }
    }
}
