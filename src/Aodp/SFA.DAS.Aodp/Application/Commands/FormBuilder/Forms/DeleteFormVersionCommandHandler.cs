using MediatR;
using SFA.DAS.Aodp.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class DeleteFormVersionCommandHandler : IRequestHandler<DeleteFormVersionCommand, BaseMediatrResponse<DeleteFormVersionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public DeleteFormVersionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<DeleteFormVersionCommandResponse>> Handle(DeleteFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<DeleteFormVersionCommandResponse>();
        response.Success = false;

        try
        {
            await _apiClient.Delete(new DeleteFormVersionApiRequest(request.FormVersionId));
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