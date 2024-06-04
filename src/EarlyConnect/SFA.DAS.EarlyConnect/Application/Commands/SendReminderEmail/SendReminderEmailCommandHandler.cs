using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.SendReminderEmail
{
    public class SendReminderEmailCommandHandler : IRequestHandler<SendReminderEmailCommand, SendReminderEmailCommandResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public SendReminderEmailCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<SendReminderEmailCommandResult> Handle(SendReminderEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<SendReminderEmailResponse>(new SendReminderEmailRequest(request.EmailReminder), true);

            response.EnsureSuccessStatusCode();

            return new SendReminderEmailCommandResult
            {
                Message = response.Body.Message,
            };
        }
    }
}


