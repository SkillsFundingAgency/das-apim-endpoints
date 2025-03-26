using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class UpdateFormVersionCommandHandler : IRequestHandler<UpdateFormVersionCommand, BaseMediatrResponse<UpdateFormVersionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public UpdateFormVersionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<UpdateFormVersionCommandResponse>> Handle(UpdateFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<UpdateFormVersionCommandResponse>();
        response.Success = false;

        try
        {
            var apiRequest = new UpdateFormVersionApiRequest()
            {
                Data = request,
                FormVersionId = request.FormVersionId,
            };
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
