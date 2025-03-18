using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetChangedQualificationsQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient) 
    : IRequestHandler<GetChangedQualificationsQuery, BaseMediatrResponse<GetChangedQualificationsQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetChangedQualificationsQueryResponse>> Handle(
        GetChangedQualificationsQuery query, 
        CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetChangedQualificationsQueryResponse>();
        response.Success = false;

        try
        {
            var result = await _apiClient.Get<BaseMediatrResponse<GetChangedQualificationsQueryResponse>>(
                new GetChangedQualificationsApiRequest());
            if (result != null && result.Value != null)
            {
                response.Value = result.Value;
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }

}
