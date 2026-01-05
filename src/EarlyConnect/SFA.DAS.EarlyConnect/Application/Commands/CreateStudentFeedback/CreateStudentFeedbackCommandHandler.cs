using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateStudentFeedback
{
    public class CreateStudentFeedbackCommandHandler : IRequestHandler<CreateStudentFeedbackCommand, CreateStudentFeedbackCommandResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateStudentFeedbackCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateStudentFeedbackCommandResult> Handle(CreateStudentFeedbackCommand request, CancellationToken cancellationToken)
        {

            var response = await _apiClient.PostWithResponseCode<CreateStudentFeedbackResponse>(new CreateStudentFeedbackRequest(request.StudentFeedbackList), true);

            response.EnsureSuccessStatusCode();

            return new CreateStudentFeedbackCommandResult
            {
                Message = response.Body.Message
            };
        }
    }
}
