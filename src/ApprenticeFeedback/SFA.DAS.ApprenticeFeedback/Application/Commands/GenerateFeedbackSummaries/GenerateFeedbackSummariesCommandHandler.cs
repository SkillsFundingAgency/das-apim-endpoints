using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
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
            var response = await _feedbackApiClient.PostWithResponseCode<object>(new GenerateFeedbackSummariesRequest(), false);

            response.EnsureSuccessStatusCode();
            return new GenerateFeedbackSummariesResponse();
        }
    }
}
