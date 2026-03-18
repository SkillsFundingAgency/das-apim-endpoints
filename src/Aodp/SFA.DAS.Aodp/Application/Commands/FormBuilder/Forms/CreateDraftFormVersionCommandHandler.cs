using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class CreateDraftFormVersionCommandHandler : IRequestHandler<CreateDraftFormVersionCommand, BaseMediatrResponse<CreateDraftFormVersionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public CreateDraftFormVersionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<CreateDraftFormVersionCommandResponse>> Handle(CreateDraftFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<CreateDraftFormVersionCommandResponse>
        {
            Success = false
        };

        try
        {
            var apiRequest = new CreateDraftFormVersionApiRequest(request.FormId);
            var result = await _apiClient.PutWithResponseCode<CreateDraftFormVersionCommandResponse>(apiRequest);

            result.EnsureSuccessStatusCode();

            response.Value.FormVersionId = result.Body.FormVersionId;
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
