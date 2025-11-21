using Azure.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class CreateApplicationMessageCommandHandler : IRequestHandler<CreateApplicationMessageCommand, BaseMediatrResponse<CreateApplicationMessageCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly INotificationService _notificationService;
    private readonly IOptions<AodpConfiguration> _options;
    private readonly ILogger<CreateApplicationMessageCommandHandler> _logger;

    public CreateApplicationMessageCommandHandler(
        IAodpApiClient<AodpApiConfiguration> apiClient, 
        INotificationService notificationService, 
        IOptions<AodpConfiguration> options,
        ILogger<CreateApplicationMessageCommandHandler> logger)
    {
        _apiClient = apiClient;
        _notificationService = notificationService;
        _options = options;
        _logger = logger;
    }

    public async Task<BaseMediatrResponse<CreateApplicationMessageCommandResponse>> Handle(CreateApplicationMessageCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<CreateApplicationMessageCommandResponse>
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.PostWithResponseCode<CreateApplicationMessageCommandResponse>(new CreateApplicationMessageApiRequest()
            {
                ApplicationId = request.ApplicationId,
                Data = request
            });

            await SendEmail(result.Body.Notifications);
            
            response.Value.Id = result.Body.Id;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }

    private async Task SendEmail(List<NotificationDefinition> notifications)
    {
        if (notifications.Any())
        {
            foreach (var notification in notifications)
            {
                var templateId = _options.Value.NotificationTemplates.FirstOrDefault(p => p.TemplateName == notification.TemplateName)?.TemplateId;

                if (templateId == null)
                {
                    _logger.LogWarning(
                        "No TemplateId configured for TemplateName {TemplateName} when sending notification ",
                        notification.TemplateName);

                    continue; 
                }

                var emailAddress = GetRecipientEmailAddress(notification);

                if (string.IsNullOrWhiteSpace(emailAddress))
                {
                    _logger.LogWarning(
                        "No email address resolved for Recipient Type{RecipientKind} when sending notification ",
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
                    { "QFASTBaseUrl", baseUrl }
                };

                if (templateId != null)
                {
                    await _notificationService.Send(new SendEmailCommand(templateId.ToString(), emailAddress, emailData));
                }
            }
        }
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
