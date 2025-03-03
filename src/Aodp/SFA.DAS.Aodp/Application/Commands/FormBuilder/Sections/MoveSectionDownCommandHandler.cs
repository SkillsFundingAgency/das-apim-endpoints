using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class MoveSectionDownCommandHandler : IRequestHandler<MoveSectionDownCommand, BaseMediatrResponse<MoveSectionDownCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public MoveSectionDownCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<MoveSectionDownCommandResponse>> Handle(MoveSectionDownCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<MoveSectionDownCommandResponse>()
        {
            Success = false
        };

        try
        {
            await _apiClient.Put(new MoveSectionDownApiRequest()
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
