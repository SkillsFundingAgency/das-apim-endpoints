
using MediatR;
using Newtonsoft.Json.Linq;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Queries.Rollover
{
    public class GetRolloverCandidatesForExportQueryHandler : IRequestHandler<GetRolloverCandidatesForExportQuery, BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public GetRolloverCandidatesForExportQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>> Handle(GetRolloverCandidatesForExportQuery request, CancellationToken cancellationToken)
        {
            
            var response = new BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>();

            try
            {
                var result = await _apiClient.Get<BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>>(new GetRolloverCandidatesForExportApiRequest()
                {
                    RolloverWorkflowRunId = request.RolloverWorkflowRunId
                });

                response.Value = result.Value;
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
}
