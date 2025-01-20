using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Sections;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;

public class UpdateSectionCommandHandler : IRequestHandler<UpdateSectionCommand, UpdateSectionCommandResponse>
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public UpdateSectionCommandHandler(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<UpdateSectionCommandResponse> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdateSectionCommandResponse()
        {
            Success = false
        };

        try
        {
            var apiRequestData = _mapper.Map<UpdateSectionApiRequest.Section>(request.Data);
            var apiRequest = new UpdateSectionApiRequest(request.FormVersionId, apiRequestData);
            var result = await _apiClient.Put<UpdateSectionApiResponse>(apiRequest);
            _mapper.Map(result.Data, response.Data);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
