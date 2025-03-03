using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class UnpublishFormVersionCommandHandler : IRequestHandler<UnpublishFormVersionCommand, BaseMediatrResponse<UnpublishFormVersionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public UnpublishFormVersionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<UnpublishFormVersionCommandResponse>> Handle(UnpublishFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<UnpublishFormVersionCommandResponse>();
        response.Success = false;

        try
        {
            var apiRequest = new UnpublishFormVersionApiRequest(request.FormVersionId);
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
