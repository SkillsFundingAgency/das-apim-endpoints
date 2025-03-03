using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Forms;

public class GetFormVersionByIdQueryHandler : IRequestHandler<GetFormVersionByIdQuery, BaseMediatrResponse<GetFormVersionByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


    public GetFormVersionByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;

    }

    public async Task<BaseMediatrResponse<GetFormVersionByIdQueryResponse>> Handle(GetFormVersionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetFormVersionByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetFormVersionByIdQueryResponse>(new GetFormVersionByIdApiRequest(request.FormVersionId));
            response.Value = result;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
