using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class DeleteSectionCommandHandler : IRequestHandler<DeleteSectionCommand, BaseMediatrResponse<DeleteSectionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public DeleteSectionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<DeleteSectionCommandResponse>> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<DeleteSectionCommandResponse>()
        {
            Success = false
        };

        try
        {
            await _apiClient.Delete(new DeleteSectionApiRequest()
            {
                SectionId = request.SectionId,
                FormVersionId = request.FormVersionId
            });
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
