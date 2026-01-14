using MediatR;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

public class SubmitApplicationCommandHandler : IRequestHandler<SubmitApplicationCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly IEmailService _notificationEmailService;
    public SubmitApplicationCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient, IEmailService notificationEmailService)
    {
        _apiClient = apiClient;
        _notificationEmailService = notificationEmailService;
    }


    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.PutWithResponseCode<SubmitApplicationCommandResponse>(
                new SubmitApplicationApiRequest
                {
                    ApplicationId = request.ApplicationId,
                    Data = request
                });

            await _notificationEmailService.SendAsync(result.Body?.Notifications);

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
