using AutoMapper;
using MediatR;
using SFA.DAS.AODP.Api;
using SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Forms;

public class GetAllFormVersionsQueryHandler : IRequestHandler<GetAllFormVersionsQuery, GetAllFormVersionsQueryResponse>
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public GetAllFormVersionsQueryHandler(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
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