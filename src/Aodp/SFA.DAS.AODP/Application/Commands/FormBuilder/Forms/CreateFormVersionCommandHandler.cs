using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;

public class CreateFormVersionCommandHandler : IRequestHandler<CreateFormVersionCommand, CreateFormVersionCommandResponse>
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public CreateFormVersionCommandHandler(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<CreateFormVersionCommandResponse> Handle(CreateFormVersionCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateFormVersionCommandResponse
        {
            Success = false
        };

        try
        {
            var apiRequestData = _mapper.Map<CreateFormVersionApiRequest.FormVersion>(request.Data);
            var result = await _apiClient.PostWithResponseCode<CreateFormVersionApiResponse>(new CreateFormVersionApiRequest(apiRequestData));
            _mapper.Map(result!.Data, response.Data);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
