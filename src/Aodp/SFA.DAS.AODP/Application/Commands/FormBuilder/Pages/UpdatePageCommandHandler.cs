using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand, UpdatePageCommandResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly IMapper _mapper;

    public UpdatePageCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<UpdatePageCommandResponse> Handle(UpdatePageCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdatePageCommandResponse()
        {
            Success = false
        };

        try
        {
            var apiRequestData = _mapper.Map<UpdatePageApiRequest.Page>(request.Data);
            var apiRequest = new UpdatePageApiRequest(request.PageId, apiRequestData);
            var result = await _apiClient.PutWithResponseCode<UpdatePageApiResponse>(apiRequest);
            response.Data = _mapper.Map<UpdatePageCommandResponse.Page>(result.Body.Data);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.Success = false;
        }

        return response;
    }
}
