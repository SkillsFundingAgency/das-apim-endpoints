using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;

public class CreateFormVersionCommandHandler : IRequestHandler<CreateFormVersionCommand, CreateFormVersionCommandResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly IMapper _mapper;

    public CreateFormVersionCommandHandler(IAodpApiClient<AodpApiConfiguration> aodpApiClient, IMapper mapper)
    {
        _apiClient = aodpApiClient;
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
            _mapper.Map(result.Body.Data, response.Data);
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
