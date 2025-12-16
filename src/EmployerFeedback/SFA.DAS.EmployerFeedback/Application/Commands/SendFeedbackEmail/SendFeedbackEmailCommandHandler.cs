using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Commands.SendFeedbackEmail
{
    public class SendFeedbackEmailCommandHandler : IRequestHandler<SendFeedbackEmailCommand>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<SendFeedbackEmailCommandHandler> _logger;

        public SendFeedbackEmailCommandHandler(
            INotificationService notificationService,
            ILogger<SendFeedbackEmailCommandHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(SendFeedbackEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing SendFeedbackEmail for template: {TemplateId}", request.Request.TemplateId);

            var tokens = CreateEmailTokens(request.Request);

            var emailCommand = new SendEmailCommand(
                templateId: request.Request.TemplateId.ToString(),
                recipientsAddress: request.Request.Email,
                tokens: tokens);

            await _notificationService.Send(emailCommand);

            _logger.LogInformation("Successfully sent email notification for template {TemplateId}", request.Request.TemplateId);
        }

        private Dictionary<string, string> CreateEmailTokens(Models.SendFeedbackEmailRequest request)
        {
            return new Dictionary<string, string>
            {
                { "contact", request.Contact },
                { "employername", request.EmployerName },
                { "accounthashedid", request.AccountHashedId },
                { "feedbackbaseurl", request.FeedbackBaseUrl },
                { "accountsbaseurl", request.AccountsBaseUrl },
            };
        }
    }
}