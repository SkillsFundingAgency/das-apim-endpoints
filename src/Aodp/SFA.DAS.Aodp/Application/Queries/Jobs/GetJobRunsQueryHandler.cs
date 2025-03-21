using Azure.Core;
using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Jobs;

public class GetJobRunsQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient) : IRequestHandler<GetJobRunsQuery, BaseMediatrResponse<GetJobRunsQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetJobRunsQueryResponse>> Handle(GetJobRunsQuery query, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetJobRunsQueryResponse>();
        response.Success = false;

        try
        {
            var result = await _apiClient.GetWithResponseCode<GetJobRunsQueryResponse>(new GetJobRunsApiRequest() { JobName = query.JobName });
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

