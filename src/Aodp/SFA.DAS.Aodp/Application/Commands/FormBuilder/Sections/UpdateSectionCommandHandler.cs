using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class UpdateSectionCommandHandler : IRequestHandler<UpdateSectionCommand, BaseMediatrResponse<UpdateSectionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public UpdateSectionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<UpdateSectionCommandResponse>> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<UpdateSectionCommandResponse>()
        {
            Success = false
        };

        try
        {
            var apiRequest = new UpdateSectionApiRequest()
            {
                FormVersionId = request.FormVersionId,
                SectionId = request.Id,
                Data = request
            };

            await _apiClient.Put(apiRequest);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
