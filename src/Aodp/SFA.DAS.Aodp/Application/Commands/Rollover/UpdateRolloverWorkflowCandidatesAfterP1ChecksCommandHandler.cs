using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Commands.Rollover;

public class UpdateRolloverWorkflowCandidatesAfterP1ChecksCommandHandler : IRequestHandler<UpdateRolloverWorkflowCandidatesAfterP1ChecksCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public UpdateRolloverWorkflowCandidatesAfterP1ChecksCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(UpdateRolloverWorkflowCandidatesAfterP1ChecksCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>();
        try
        {
            await _apiClient.PostWithResponseCode<EmptyResponse>(new UpdateRolloverWorkflowCandidatesAfterP1ChecksApiRequest());
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
