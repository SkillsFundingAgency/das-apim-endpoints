using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Rollover
{
    public class GetRolloverCandidatesQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient) : IRequestHandler<GetRolloverCandidatesQuery, BaseMediatrResponse<GetRolloverCandidatesQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

        public async Task<BaseMediatrResponse<GetRolloverCandidatesQueryResponse>> Handle(GetRolloverCandidatesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetRolloverCandidatesQueryResponse>();

            try
            {
                var result = await _apiClient.Get<GetRolloverCandidatesQueryResponse>(new GetRolloverCandidatesApiRequest());

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
