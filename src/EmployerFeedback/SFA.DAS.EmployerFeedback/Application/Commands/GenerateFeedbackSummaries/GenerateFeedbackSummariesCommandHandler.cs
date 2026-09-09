using MediatR;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Commands.GenerateFeedbackSummaries
{
    public class GenerateFeedbackSummariesCommandHandler : IRequestHandler<GenerateFeedbackSummariesCommand>
    {
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _apiClient;

        public GenerateFeedbackSummariesCommandHandler(IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task Handle(GenerateFeedbackSummariesCommand command, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<object>(new GenerateFeedbackSummariesRequest(), false);

            response.EnsureSuccessStatusCode();
        }
    }
}
