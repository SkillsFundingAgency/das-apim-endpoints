using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.Encoding;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification
{
    public class SendResponseNotificationCommandHandler : IRequestHandler<SendResponseNotificationCommand, Unit>
    {
        private readonly INotificationService _notificationService;
        private readonly IOptions<EmployerRequestApprenticeTrainingConfiguration> _options;
        private readonly IEncodingService _encodingService;

        public SendResponseNotificationCommandHandler(
            INotificationService notificationService,
            IEncodingService encodingService,
            IOptions<EmployerRequestApprenticeTrainingConfiguration> options)
        {
            _notificationService = notificationService;
            _encodingService = encodingService;
            _options = options;
        }

        public async Task<Unit> Handle(SendResponseNotificationCommand command, CancellationToken cancellationToken)
        { 

        var templateId = _options.Value.NotificationTemplates.FirstOrDefault(
            p => p.TemplateName == EmailTemplateNames.RATEmployerResponseNotification)?.TemplateId;
        if (templateId != null)
        {
            await _notificationService.Send(
                new SendEmailCommand(templateId.ToString(), command.EmailAddress, GetResponseEmailData(command)));
        }
            return Unit.Value;
        }

        public Dictionary<string, string> GetResponseEmailData(SendResponseNotificationCommand command)
        {
            var courseList = new StringBuilder();
            foreach (var course in command.Standards)
            {
                courseList.Append($"* {course.StandardTitle} (level{course.StandardLevel})\n");
            }

            var encodedAccountId = _encodingService.Encode(command.AccountId, EncodingType.AccountId);
            var manageRequestsLink = string.Format(command.ManageRequestsLink, encodedAccountId);
            
            var manageNotificationSettingsLink = $"{command.ManageNotificationSettingsLink}";

            return new Dictionary<string, string>
            {
                { "user_name",  command.FirstName},
                { "course_level_bullet_points", courseList.ToString()},
                { "dashboard_url", manageRequestsLink },
                { "unsubscribe_url", manageNotificationSettingsLink }
            };
        }
    }
}
