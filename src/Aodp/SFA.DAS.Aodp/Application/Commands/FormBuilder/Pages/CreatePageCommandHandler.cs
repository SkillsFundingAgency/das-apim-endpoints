using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class CreatePageCommandHandler : IRequestHandler<CreatePageCommand, CreatePageCommandResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    

    public CreatePageCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
       
    }

    public async Task<CreatePageCommandResponse> Handle(CreatePageCommand request, CancellationToken cancellationToken)
    {
        var response = new CreatePageCommandResponse();
        try
        {
            var apiRequestData = new CreatePageApiRequest()
            {
                Data = new CreatePageApiRequest.Page()
                {
                    Description = request.Description,
                    Title = request.Title
                },
                SectionId = request.SectionId,
                FormVersionId = request.FormVersionId
            };

            var result = await _apiClient.PostWithResponseCode<CreatePageApiResponse>(apiRequestData);
            result.EnsureSuccessStatusCode();
            response.Id = result!.Body.Id;
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
