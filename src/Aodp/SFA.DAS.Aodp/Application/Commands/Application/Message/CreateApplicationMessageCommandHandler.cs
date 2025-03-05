using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Message;

public class CreateApplicationMessageCommandHandler : IRequestHandler<CreateApplicationMessageCommand, BaseMediatrResponse<CreateApplicationMessageCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    public CreateApplicationMessageCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
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
                ApplicationId = (Guid)request.ApplicationId,
                Data = request
            });

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
