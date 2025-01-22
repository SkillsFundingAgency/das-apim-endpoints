using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Forms;

public class GetAllFormVersionsQueryHandler : IRequestHandler<GetAllFormVersionsQuery, GetAllFormVersionsQueryResponse>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
    private readonly IMapper _mapper;

    public GetAllFormVersionsQueryHandler(IAodpApiClient<AodpApiConfiguration> aodpApiClient, IMapper mapper)
    {
        _apiClient = aodpApiClient;
        _mapper = mapper;
    }

    public async Task<GetAllFormVersionsQueryResponse> Handle(GetAllFormVersionsQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllFormVersionsQueryResponse
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.Get<GetAllFormVersionsApiResponse>(new GetAllFormVersionsApiRequest());
            response.Data = _mapper.Map<List<GetAllFormVersionsQueryResponse.FormVersion>>(result.Data);
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