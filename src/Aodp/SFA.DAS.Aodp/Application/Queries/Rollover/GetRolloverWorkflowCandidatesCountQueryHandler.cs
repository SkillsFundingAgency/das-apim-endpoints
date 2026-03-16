using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;
public class GetRolloverWorkflowCandidatesCountQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient) : IRequestHandler<GetRolloverWorkflowCandidatesCountQuery, BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>> Handle(GetRolloverWorkflowCandidatesCountQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>();
        try
        {
            var result = await _apiClient.Get<BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>>(new GetRolloverWorkflowCandidatesCountApiRequest());

            response.Value.TotalRecords = result.Value.TotalRecords;
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
