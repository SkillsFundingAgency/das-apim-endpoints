using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;

public class DeleteFormVersionCommandHandler : IRequestHandler<DeleteFormVersionCommand, DeleteFormVersionCommandResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public DeleteFormVersionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<DeleteFormVersionCommandResponse> Handle(DeleteFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new DeleteFormVersionCommandResponse();
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