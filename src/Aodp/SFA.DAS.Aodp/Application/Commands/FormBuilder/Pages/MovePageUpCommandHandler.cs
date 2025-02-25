using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;

public class MovePageUpCommandHandler : IRequestHandler<MovePageUpCommand, BaseMediatrResponse<MovePageUpCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public MovePageUpCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<MovePageUpCommandResponse>> Handle(MovePageUpCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<MovePageUpCommandResponse>()
        {
            Success = false
        };

        try
        {
            await _apiClient.Put(new MovePageUpApiRequest()
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
