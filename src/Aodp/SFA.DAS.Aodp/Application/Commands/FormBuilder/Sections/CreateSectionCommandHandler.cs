using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Sections;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;

public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, CreateSectionCommandResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public CreateSectionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<CreateSectionCommandResponse> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateSectionCommandResponse()
        {
            Success = false
        };

        try
        {
            var apiRequest = new CreateSectionApiRequest()
            {
                Data = new CreateSectionApiRequest.Section()
                {
                    Description = request.Description,
                    Title = request.Title,

                },
                FormVersionId = request.FormVersionId,
            };

            var result = await _apiClient.PostWithResponseCode<CreateSectionApiResponse>(apiRequest);
            result.EnsureSuccessStatusCode();
            response.Id = result.Body.Id;
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
