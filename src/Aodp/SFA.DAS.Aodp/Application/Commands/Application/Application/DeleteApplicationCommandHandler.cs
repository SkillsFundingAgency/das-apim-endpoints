using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

public class DeleteApplicationCommandHandler : IRequestHandler<DeleteApplicationCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    public DeleteApplicationCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }


    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>
        {
            Success = false
        };

        try
        {
            await _apiClient.Delete(new DeleteApplicationApiRequest()
            {
                ApplicationId = request.ApplicationId
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
