using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Jobs;

public class GetJobRunsQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    : IRequestHandler<GetJobRunsQuery, BaseMediatrResponse<GetJobRunsQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetJobRunsQueryResponse>> Handle(
        GetJobRunsQuery query,
        CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetJobRunsQueryResponse>();
        response.Success = false;

        try
        {
            var result = await _apiClient.Get<BaseMediatrResponse<GetJobRunsQueryResponse>>(
                new GetJobRunsApiRequest());
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

