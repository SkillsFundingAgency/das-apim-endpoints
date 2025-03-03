using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class CreateFormVersionCommandHandler : IRequestHandler<CreateFormVersionCommand, BaseMediatrResponse<CreateFormVersionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public CreateFormVersionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<CreateFormVersionCommandResponse>> Handle(CreateFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<CreateFormVersionCommandResponse>
        {
            Success = false
        };

        try
        {
            var apiRequest = new CreateFormVersionApiRequest()
            {
                Data = request
            };
            var result = await _apiClient.PostWithResponseCode<CreateFormVersionCommandResponse>(apiRequest);
            response.Value.Id = result.Body.Id!;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
