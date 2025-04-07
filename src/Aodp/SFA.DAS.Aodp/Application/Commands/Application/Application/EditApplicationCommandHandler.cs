using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class EditApplicationCommandHandler : IRequestHandler<EditApplicationCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    public EditApplicationCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }


    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(EditApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>
        {
            Success = false
        };

        try
        {
            await _apiClient.Put(new EditApplicationApiRequest()
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
