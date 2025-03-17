using Azure.Core;
using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Jobs;

public class GetJobRunsByNameQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient) : IRequestHandler<GetJobRunsByNameQuery, BaseMediatrResponse<GetJobRunsByNameQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetJobRunsByNameQueryResponse>> Handle(GetJobRunsByNameQuery query, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetJobRunsByNameQueryResponse>();
        response.Success = false;

        try
        {
            var result = await _apiClient.GetWithResponseCode<GetJobRunsByNameQueryResponse>(new GetJobRunsByNameApiRequest() { JobName = query.JobName });
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

