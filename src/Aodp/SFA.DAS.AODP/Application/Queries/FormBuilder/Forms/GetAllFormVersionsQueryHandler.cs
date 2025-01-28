using MediatR;
using SFA.DAS.Aodp.Domain.FormBuilder.Requests.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Forms;

public class GetAllFormVersionsQueryHandler : IRequestHandler<GetAllFormVersionsQuery, BaseMediatrResponse<GetAllFormVersionsQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetAllFormVersionsQueryHandler(IAodpApiClient<AodpApiConfiguration> aodpApiClient)
    {
        _apiClient = aodpApiClient;

    }

    public async Task<BaseMediatrResponse<GetAllFormVersionsQueryResponse>> Handle(GetAllFormVersionsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetAllFormVersionsQueryResponse>
        {
            Success = false
        };

        try
        {
            var result = await _apiClient.Get<GetAllFormVersionsQueryResponse>(new GetAllFormVersionsApiRequest());
            response.Value = result;
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