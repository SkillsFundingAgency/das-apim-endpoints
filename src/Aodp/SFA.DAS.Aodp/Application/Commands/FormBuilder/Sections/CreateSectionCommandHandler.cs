using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;


namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, BaseMediatrResponse<CreateSectionCommandResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public CreateSectionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<CreateSectionCommandResponse>> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<CreateSectionCommandResponse>()
        {
            Success = false
        };

        try
        {
            var apiRequest = new CreateSectionApiRequest()
            {
                Data = request,
                FormVersionId = request.FormVersionId,
            };

            var result = await _apiClient.PostWithResponseCode<CreateSectionCommandResponse>(apiRequest);
            response.Value.Id = result.Body.Id;
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
