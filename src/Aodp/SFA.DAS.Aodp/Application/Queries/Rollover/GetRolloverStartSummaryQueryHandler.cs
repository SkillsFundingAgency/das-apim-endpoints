using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetRolloverStartSummaryQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    : IRequestHandler<GetRolloverStartSummaryQuery, BaseMediatrResponse<GetRolloverStartSummaryQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetRolloverStartSummaryQueryResponse>> Handle(
        GetRolloverStartSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetRolloverStartSummaryQueryResponse>();

        try
        {
            var result = await _apiClient.Get<GetRolloverStartSummaryQueryResponse>(new GetRolloverStartSummaryApiRequest());

            response.Value = result;
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