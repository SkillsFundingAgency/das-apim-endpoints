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
            var result = await _apiClient.GetWithResponseCode<GetJobByNameQueryResponse>(new GetJobByNameApiRequest(query.JobName));
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

