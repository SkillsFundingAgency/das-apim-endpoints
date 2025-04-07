using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Jobs;

public class RequestJobRunCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient) : IRequestHandler<RequestJobRunCommand, BaseMediatrResponse<EmptyResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<EmptyResponse>> Handle(RequestJobRunCommand command, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<EmptyResponse>();
        response.Success = false;

        try
        {
            var result = await _apiClient.PostWithResponseCode<EmptyResponse>(new RequestJobRunApiRequest()
            {
                Data = command
            });
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

