using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.Application.Handlers;

public class AssignApprenticeshipIdCommandHandler(IInternalApiClient<LearnerDataInnerApiConfiguration> client, ILogger<AssignApprenticeshipIdCommand> logger)
    : IRequestHandler<AssignApprenticeshipIdCommand, bool>
{
    public async Task<bool> Handle(AssignApprenticeshipIdCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Building PATCH request to assign ApprenticeshipId");
            var request = new PatchLearnerDataApprenticeshipIdRequest(command.ProviderId, command.LearnerDataId, command.PatchRequest);

            logger.LogInformation("Calling inner api to assign ApprenticeshipId");
            var response = await client.PatchWithResponseCode(request);
            if (!string.IsNullOrWhiteSpace(response.ErrorContent))
            {
                logger.LogInformation("Assigning ApprenticeshipId returned status code {0} and error {1}", response.StatusCode, response.ErrorContent);
            }
            return (int)response.StatusCode >= 200 && (int)response.StatusCode <= 299;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred whilst calling inner api to assign ApprenticeshipId");
            throw;
        }
    }
}