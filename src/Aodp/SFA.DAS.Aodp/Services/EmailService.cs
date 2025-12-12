using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Services;
public class EmailService : IEmailService
{
    private readonly INotificationService _notificationService;
    private readonly IOptions<AodpConfiguration> _options;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        INotificationService notificationService,
        IOptions<AodpConfiguration> options,
        ILogger<EmailService> logger)
    {
        _notificationService = notificationService;
        _options = options;
        _logger = logger;
    }

    public async Task<bool> SendAsync(
        IReadOnlyCollection<NotificationDefinition> notifications,
        CancellationToken cancellationToken = default)
    {
        if (notifications == null || notifications.Count == 0)
        {
            return false;
        }

        bool emailSent = false;
        foreach (var notification in notifications)
        {
            var templateId = _options.Value.NotificationTemplates
                .FirstOrDefault(p => p.TemplateName == notification.TemplateName)
                ?.TemplateId;

            if (templateId == null)
            {
                _logger.LogWarning(
                    "No TemplateId configured for TemplateName {TemplateName} when sending notification",
                    notification.TemplateName);
                continue;
            }

            var emailAddress = GetRecipientEmailAddress(notification);

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                _logger.LogWarning(
                    "No email address resolved for RecipientKind {RecipientKind} when sending notification",
                    notification.RecipientKind);
                continue;
            }

            var baseUrl = _options.Value.QFASTBaseUrl;
            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.LogWarning(
                    "No base url configured for Aodp when sending notification");
                continue;
            }

            var emailData = new Dictionary<string, string>
            {
                ["QFASTBaseUrl"] = baseUrl
            };

            await _notificationService.Send(
                new SendEmailCommand(templateId.ToString(), emailAddress, emailData));

            emailSent = true;
        }
        return emailSent;
    }

    private string? GetRecipientEmailAddress(NotificationDefinition notification)
    {
        return notification.RecipientKind switch
        {
            NotificationRecipientKind.DirectEmail => notification.EmailAddress,
            NotificationRecipientKind.QfauMailbox => _options.Value.QfauReviewerEmailAddress,
            _ => null
        };
    }
}
