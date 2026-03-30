using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Rollover
{
    public class GetRolloverWorkflowCandidatesQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient) : IRequestHandler<GetRolloverWorkflowCandidatesQuery, BaseMediatrResponse<GetRolloverWorkflowCandidatesQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

        public async Task<BaseMediatrResponse<GetRolloverWorkflowCandidatesQueryResponse>> Handle(GetRolloverWorkflowCandidatesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetRolloverWorkflowCandidatesQueryResponse>();

            try
            {
                var result = await _apiClient.Get<GetRolloverWorkflowCandidatesQueryResponse>(new GetRolloverWorkflowCandidatesApiRequest());

                if (result != null)
                {
                    response.Value = result;
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = $"Failed to get rollover workflow candidates.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        } 
    }
}
