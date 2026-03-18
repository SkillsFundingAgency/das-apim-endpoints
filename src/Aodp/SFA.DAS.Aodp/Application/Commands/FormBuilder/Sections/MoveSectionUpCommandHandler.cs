using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class MoveSectionUpCommandHandler : IRequestHandler<MoveSectionUpCommand, BaseMediatrResponse<MoveSectionUpCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public MoveSectionUpCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<MoveSectionUpCommandResponse>> Handle(MoveSectionUpCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<MoveSectionUpCommandResponse>()
        {
            Success = false
        };

        try
        {
            await _apiClient.Put(new MoveSectionUpApiRequest()
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
