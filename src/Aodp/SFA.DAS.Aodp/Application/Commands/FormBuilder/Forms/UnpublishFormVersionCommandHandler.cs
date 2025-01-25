using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;

public class UnpublishFormVersionCommandHandler : IRequestHandler<UnpublishFormVersionCommand, UnpublishFormVersionCommandResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public UnpublishFormVersionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<UnpublishFormVersionCommandResponse> Handle(UnpublishFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new UnpublishFormVersionCommandResponse();
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
