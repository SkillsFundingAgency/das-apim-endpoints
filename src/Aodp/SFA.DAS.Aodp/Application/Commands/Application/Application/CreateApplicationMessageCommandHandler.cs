using Azure.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class CreateApplicationMessageCommandHandler : IRequestHandler<CreateApplicationMessageCommand, BaseMediatrResponse<CreateApplicationMessageCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly IOptions<AodpConfiguration> _options;
    private readonly ILogger<CreateApplicationMessageCommandHandler> _logger;
    private readonly IEmailService _notificationEmailService;

    public CreateApplicationMessageCommandHandler(
        IAodpApiClient<AodpApiConfiguration> apiClient,  
        IOptions<AodpConfiguration> options,
        ILogger<CreateApplicationMessageCommandHandler> logger,
        IEmailService notificationEmailService)
    {
        _apiClient = apiClient;
        _options = options;
        _logger = logger;
        _notificationEmailService = notificationEmailService;
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

            await _notificationEmailService.SendAsync(result.Body.Notifications);
            
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
}
