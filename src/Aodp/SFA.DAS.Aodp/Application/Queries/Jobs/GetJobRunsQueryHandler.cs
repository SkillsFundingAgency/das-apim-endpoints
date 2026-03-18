using Azure.Core;
using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
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
            var result = await _apiClient.GetWithResponseCode<GetJobRunsApiResponse>(new GetJobRunsApiRequest() { JobName = query.JobName });
            result.EnsureSuccessStatusCode();

            response.Value = new GetJobRunsQueryResponse()
            {
                 JobRuns = result.Body.JobRuns,
            };
            response.Success = true;

        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}

