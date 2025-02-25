using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class MoveFormDownCommandHandler : IRequestHandler<MoveFormDownCommand, BaseMediatrResponse<MoveFormDownCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public MoveFormDownCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<MoveFormDownCommandResponse>> Handle(MoveFormDownCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<MoveFormDownCommandResponse>();
        response.Success = false;

        try
        {
            var apiRequest = new MoveFormDownApiRequest(request.FormId);
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
