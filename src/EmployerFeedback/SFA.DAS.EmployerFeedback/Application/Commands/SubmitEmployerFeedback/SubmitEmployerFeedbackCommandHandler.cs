using MediatR;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Commands.SubmitEmployerFeedback
{
    public class SubmitEmployerFeedbackCommandHandler : IRequestHandler<SubmitEmployerFeedbackCommand>
    {
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _apiClient;

        public SubmitEmployerFeedbackCommandHandler(IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task Handle(SubmitEmployerFeedbackCommand command, CancellationToken cancellationToken)
        {
            var request = new SubmitEmployerFeedbackRequest(command);

            var response = await _apiClient
                .PostWithResponseCode<SubmitEmployerFeedbackRequestData, object>(request,false);

            response.EnsureSuccessStatusCode();
        }
    }
}
