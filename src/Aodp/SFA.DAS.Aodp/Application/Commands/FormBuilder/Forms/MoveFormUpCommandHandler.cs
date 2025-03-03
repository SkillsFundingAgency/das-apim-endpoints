using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class MoveFormUpCommandHandler : IRequestHandler<MoveFormUpCommand, BaseMediatrResponse<MoveFormUpCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public MoveFormUpCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<MoveFormUpCommandResponse>> Handle(MoveFormUpCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<MoveFormUpCommandResponse>();
        response.Success = false;

        try
        {
            var apiRequest = new MoveFormUpApiRequest(request.FormId);
            await _apiClient.Put(apiRequest);
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
