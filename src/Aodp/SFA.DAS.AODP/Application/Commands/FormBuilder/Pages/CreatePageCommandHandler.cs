using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class CreatePageCommandHandler : IRequestHandler<CreatePageCommand, CreatePageCommandResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly IMapper _mapper;

    public CreatePageCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<CreatePageCommandResponse> Handle(CreatePageCommand request, CancellationToken cancellationToken)
    {
        var response = new CreatePageCommandResponse();
        try
        {
            var apiRequestData = _mapper.Map<CreatePageApiRequest.Page>(request.Data);
            var result = await _apiClient.PostWithResponseCode<CreatePageApiResponse>(new CreatePageApiRequest(apiRequestData));
            response.Data = _mapper.Map<CreatePageCommandResponse.Page>(result.Body.Data);
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
