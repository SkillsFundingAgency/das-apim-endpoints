using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CancelEmployerRequest
{
    public class CancelEmployerRequestCommandHandler : IRequestHandler<CancelEmployerRequestCommand>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;
        private readonly INotificationService _notificationService;
        private readonly IOptions<EmployerRequestApprenticeTrainingConfiguration> _options;

        public CancelEmployerRequestCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient,
            INotificationService notificationService, IOptions<EmployerRequestApprenticeTrainingConfiguration> options)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
            _notificationService = notificationService;
            _options = options;
        }

        public async Task Handle(CancelEmployerRequestCommand command, CancellationToken cancellationToken)
        {
            var request = new PutCancelEmployerRequestRequest(command.EmployerRequestId, new PutCancelEmployerRequestRequestData
            {
                CancelledBy = command.CancelledBy
            });

            var response = await _requestApprenticeTrainingApiClient
                .PutWithResponseCode<PutCancelEmployerRequestRequestData, NullResponse>(request);

            response.EnsureSuccessStatusCode();

            var templateId = _options.Value.NotificationTemplates.FirstOrDefault(p => p.TemplateName == EmailTemplateNames.RATEmployerCancelConfirmation)?.TemplateId;
            if (templateId != null)
            {
                await _notificationService.Send(new SendEmailCommand(
                    templateId.ToString(),
                    command.CancelledByEmail,
                    GetEmailData(command)));
            }
        }

        public Dictionary<string, string> GetEmailData(CancelEmployerRequestCommand command)
        {
            return new Dictionary<string, string>
            {
                { "course_level", command.CourseLevel },
                { "user_name", command.CancelledByFirstName },
                { "dashboard_url", command.DashboardUrl }
            };
        }
    }
}
