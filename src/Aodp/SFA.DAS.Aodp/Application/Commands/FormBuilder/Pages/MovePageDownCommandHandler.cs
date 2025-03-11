using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;

public class MovePageDownCommandHandler : IRequestHandler<MovePageDownCommand, BaseMediatrResponse<MovePageDownCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public MovePageDownCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<MovePageDownCommandResponse>> Handle(MovePageDownCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<MovePageDownCommandResponse>()
        {
            Success = false
        };

        try
        {
            await _apiClient.Put(new MovePageDownApiRequest()
            {
                FormVersionId = request.FormVersionId,
                PageId = request.PageId,
                SectionId = request.SectionId
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
