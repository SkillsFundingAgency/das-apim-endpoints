using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.Aodp.Application.Commands.Application.Application;
public class WithdrawApplicationCommandHandler : IRequestHandler<WithdrawApplicationCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly IEmailService _notificationEmailService;
    private readonly ILogger<WithdrawApplicationCommandHandler> _logger;
    public WithdrawApplicationCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient, 
        IEmailService notificationEmailService, ILogger<WithdrawApplicationCommandHandler> logger)
    {
        _apiClient = apiClient;
        _notificationEmailService = notificationEmailService;
        _logger = logger;
    }

    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.PostWithResponseCode<WithdrawApplicationCommandResponse>(
                new WithdrawApplicationApiRequest
                {
                    ApplicationId = request.ApplicationId,
                    Data = request
                });

            await _notificationEmailService.SendAsync(result.Body?.Notifications);

            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
               "Error withdrawing application {ApplicationId} for {WithdrawnBy}",
               request.ApplicationId,
               request.WithdrawnBy);
            response.Success = false;
            response.ErrorMessage = "There was a problem withdrawing the application.";
        }

        return response;
    }
}
