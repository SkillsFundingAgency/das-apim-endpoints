using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Pages;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, DeletePageCommandResponse>
{
    private readonly IApiClient _apiClient;

    public DeletePageCommandHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<DeletePageCommandResponse> Handle(DeletePageCommand request, CancellationToken cancellationToken)
    {
        var response = new DeletePageCommandResponse();

        try
        {
            var result = await _apiClient.Delete<DeletePageApiResponse>(new DeletePageApiRequest(request.PageId));
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
