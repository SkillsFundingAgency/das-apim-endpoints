using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;

public class DeleteFormVersionCommandHandler : IRequestHandler<DeleteFormVersionCommand, DeleteFormVersionCommandResponse>
{
    private readonly IApiClient _apiClient;

    public DeleteFormVersionCommandHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<DeleteFormVersionCommandResponse> Handle(DeleteFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new DeleteFormVersionCommandResponse();
        response.Success = false;

        try
        {
            var result = await _apiClient.Delete<DeleteFormVersionApiResponse>(new DeleteFormVersionApiRequest(request.FormVersionId));
            response.Data = result!.Data;
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