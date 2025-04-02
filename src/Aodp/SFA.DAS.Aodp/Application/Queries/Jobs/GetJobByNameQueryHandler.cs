using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Jobs;

public class GetJobByNameQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient) : IRequestHandler<GetJobByNameQuery, BaseMediatrResponse<GetJobByNameQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetJobByNameQueryResponse>> Handle(GetJobByNameQuery query, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetJobByNameQueryResponse>();
        response.Success = false;

        try
        {
            var result = await _apiClient.GetWithResponseCode<GetJobByNameApiResponse>(new GetJobByNameApiRequest(query.JobName));
            result.EnsureSuccessStatusCode();

            response.Value = new GetJobByNameQueryResponse()
            {
                Id = result.Body.Id,
                Status = result.Body.Status,
                Enabled = result.Body.Enabled,
                LastRunTime = result.Body.LastRunTime,
                Name = result.Body.Name,
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

