using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.Aodp.Services;
using System.Net;

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
            var result = await _apiClient.PutWithResponseCode<UpdateFormVersionCommandResponse>(apiRequest);

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                response.ErrorCode = ErrorCodes.BadRequest;
                response.ErrorMessage = result.ErrorContent;
                return response;
            }

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
