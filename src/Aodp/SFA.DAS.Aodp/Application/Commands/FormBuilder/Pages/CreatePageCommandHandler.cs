using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;

public class CreatePageCommandHandler : IRequestHandler<CreatePageCommand, BaseMediatrResponse<CreatePageCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    

    public CreatePageCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
       
    }

    public async Task<BaseMediatrResponse<CreatePageCommandResponse>> Handle(CreatePageCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<CreatePageCommandResponse>();
        try
        {
            var apiRequestData = new CreatePageApiRequest()
            {
                Data = request,
                SectionId = request.SectionId,
                FormVersionId = request.FormVersionId
            };

            var result = await _apiClient.PostWithResponseCode<CreatePageCommandResponse>(apiRequestData);
            response.Value.Id = result!.Body.Id;
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
