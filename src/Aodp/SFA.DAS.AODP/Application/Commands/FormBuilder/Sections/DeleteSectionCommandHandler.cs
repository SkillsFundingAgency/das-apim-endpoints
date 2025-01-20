using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Sections;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;

public class DeleteSectionCommandHandler : IRequestHandler<DeleteSectionCommand, DeleteSectionCommandResponse>
{
    private readonly IApiClient _apiClient;

    public DeleteSectionCommandHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<DeleteSectionCommandResponse> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
    {
        var response = new DeleteSectionCommandResponse()
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.Delete<DeleteSectionApiResponse>(new DeleteSectionApiRequest(request.SectionId));
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
