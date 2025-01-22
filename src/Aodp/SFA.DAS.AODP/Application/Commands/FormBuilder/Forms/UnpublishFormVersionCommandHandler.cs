using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;
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
            var result = await _apiClient.PutWithResponseCode<UnpublishFormVersionApiResponse>(apiRequest);
            response = new UnpublishFormVersionCommandResponse();
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
