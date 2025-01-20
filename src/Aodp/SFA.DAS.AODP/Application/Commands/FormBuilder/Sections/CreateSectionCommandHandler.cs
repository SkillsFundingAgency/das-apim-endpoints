using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Sections;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;

public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, CreateSectionCommandResponse>
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public CreateSectionCommandHandler(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<CreateSectionCommandResponse> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateSectionCommandResponse()
        {
            Success = false
        };

        try
        {
            var apiRequestData = _mapper.Map<CreateSectionApiRequest.Section>(request.Data);
            var result = await _apiClient.PostWithResponseCode<CreateSectionApiResponse>(new CreateSectionApiRequest(apiRequestData));
            _mapper.Map(result!.Data, response.Data);
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
