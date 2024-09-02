using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.ProviderRequestApprenticeTraining.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse
{
    public class SubmitProviderResponseCommandHandler : IRequestHandler<SubmitProviderResponseCommand, SubmitProviderResponseResponse>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;
        private readonly INotificationService _notificationService;
        private readonly IOptions<ProviderRequestApprenticeTrainingConfiguration> _options;

        public SubmitProviderResponseCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient,
            INotificationService notificationService, IOptions<ProviderRequestApprenticeTrainingConfiguration> options)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
            _notificationService = notificationService;
            _options = options;
        }

        public async Task<SubmitProviderResponseResponse> Handle(SubmitProviderResponseCommand command, CancellationToken cancellationToken)
        {
            var submitProviderResponseRequest = new SubmitProviderResponseRequest(new SubmitProviderResponseRequestData
            {
                EmployerRequestIds = command.EmployerRequestIds,
                Ukprn = command.Ukprn,
                Website = command.Website,
                Email = command.Email,
                Phone = command.Phone,

            });

            var response = await _requestApprenticeTrainingApiClient
                .PostWithResponseCode<SubmitProviderResponseRequestData, SubmitProviderResponseResponse>(submitProviderResponseRequest);

            response.EnsureSuccessStatusCode();

            var templateId = _options.Value.NotificationTemplates.FirstOrDefault(p => p.TemplateName == "RATProviderResponseConfirmation")?.TemplateId;
            if (templateId != null)
            {
                await _notificationService.Send(new SendEmailCommand(templateId.ToString(), command.CurrentUserEmail, new Dictionary<string, string>()));
            }

            return response.Body;
        }
    }
}
