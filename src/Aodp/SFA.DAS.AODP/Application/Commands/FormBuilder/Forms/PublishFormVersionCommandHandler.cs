using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class PublishFormVersionCommandHandler : IRequestHandler<PublishFormVersionCommand, BaseMediatrResponse<PublishFormVersionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public PublishFormVersionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<PublishFormVersionCommandResponse>> Handle(PublishFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<PublishFormVersionCommandResponse>();
        response.Success = false;

        try
        {
            var apiRequest = new PublishFormVersionApiRequest(request.FormVersionId);
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
