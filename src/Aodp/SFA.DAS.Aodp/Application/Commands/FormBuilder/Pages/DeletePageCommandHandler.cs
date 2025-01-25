using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, DeletePageCommandResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public DeletePageCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<DeletePageCommandResponse> Handle(DeletePageCommand request, CancellationToken cancellationToken)
    {
        var response = new DeletePageCommandResponse();

        try
        {
            await _apiClient.Delete(new DeletePageApiRequest()
            {
                FormVersionId = request.FormVersionId,
                PageId = request.PageId,
                SectionId = request.SectionId
            });
            response.Data = true;
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
