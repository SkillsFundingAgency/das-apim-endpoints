using MediatR;
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

    public CreateApplicationMessageCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient, INotificationService notificationService, IOptions<AodpConfiguration> options)
    {
        _apiClient = apiClient;
        _notificationService = notificationService;
        _options = options;
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

            await SendEmail(request);

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

    private async Task SendEmail(CreateApplicationMessageCommand request)
    {
        //TODO : Do we need to get the message and who it was shared with before deciding who to notify?  
        var templateId = _options.Value.NotificationTemplates.FirstOrDefault(p => p.TemplateName == EmailTemplateNames.QFASTApplicationMessageSentNotification)?.TemplateId;
        var emailAddress = _options.Value.QfauReviewerEmailAddress;
        var emailData = new Dictionary<string, string>();

        if (templateId != null)
        {
            await _notificationService.Send(new SendEmailCommand(templateId.ToString(), emailAddress, emailData));
        }
    }
}
