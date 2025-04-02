using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class MarkAllMessagesAsReadCommandHandler : IRequestHandler<MarkAllMessagesAsReadCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    public MarkAllMessagesAsReadCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }


    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(MarkAllMessagesAsReadCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>
        {
            Success = false
        };

        try
        {
            await _apiClient.Put(new MarkAllMessagesAsReadApiRequest()
            {
                ApplicationId = request.ApplicationId,
                Data = request
            });

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