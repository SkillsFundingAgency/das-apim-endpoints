using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    public class RemovePreviousWorkflowCandidatesCommandHandler : IRequestHandler<RemovePreviousWorkflowCandidatesCommand, BaseMediatrResponse<RemovePreviousWorkflowCandidatesCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public RemovePreviousWorkflowCandidatesCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<RemovePreviousWorkflowCandidatesCommandResponse>> Handle(RemovePreviousWorkflowCandidatesCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<RemovePreviousWorkflowCandidatesCommandResponse>();

            try
            {
                var result = await _apiClient.PostWithResponseCode<RemovePreviousWorkflowCandidatesCommandResponse>(new RemovePreviousWorkflowCandidatesApiRequest()
                { 
                    Data = request
                });

                response.Value.Success = true;
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
