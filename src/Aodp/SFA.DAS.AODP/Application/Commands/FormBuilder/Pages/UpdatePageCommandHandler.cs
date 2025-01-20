using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Pages;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand, UpdatePageCommandResponse>
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public UpdatePageCommandHandler(IApiClient apiClient, IMapper mapper)
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
            var result = await _apiClient.Put<UpdatePageApiResponse>(apiRequest);
            _mapper.Map(result.Data, response.Data);
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
