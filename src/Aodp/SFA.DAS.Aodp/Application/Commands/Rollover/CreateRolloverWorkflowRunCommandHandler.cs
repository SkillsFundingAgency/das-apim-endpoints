using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    public class CreateRolloverWorkflowRunCommandHandler : IRequestHandler<CreateRolloverWorkflowRunCommand, BaseMediatrResponse<CreateRolloverWorkflowRunCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public CreateRolloverWorkflowRunCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<CreateRolloverWorkflowRunCommandResponse>> Handle(CreateRolloverWorkflowRunCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<CreateRolloverWorkflowRunCommandResponse>();

            try
            {
                var result = await _apiClient.PostWithResponseCode<CreateRolloverWorkflowRunCommandResponse>(new CreateRolloverWorkflowRunApiRequest()
                { 
                    Data = request
                });

                response.Value.RolloverWorkflowRunId = result.Body.RolloverWorkflowRunId;
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
