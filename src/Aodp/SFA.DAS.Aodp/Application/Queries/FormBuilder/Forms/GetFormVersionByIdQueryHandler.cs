using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

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
            var result = await _apiClient.GetWithResponseCode<GetFormVersionByIdQueryResponse>(new GetFormVersionByIdApiRequest(request.FormVersionId));
            result.EnsureSuccessStatusCode();
            response.Value = result.Body;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
