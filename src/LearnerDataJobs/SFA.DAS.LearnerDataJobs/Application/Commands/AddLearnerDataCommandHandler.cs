using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.Application.Commands;

public class AddLearnerDataCommandHandler(IInternalApiClient<LearnerDataInnerApiConfiguration> client, ILogger<AddLearnerDataCommandHandler> logger)
    : IRequestHandler<AddLearnerDataCommand, bool>
{
    public async Task<bool> Handle(AddLearnerDataCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace("Calling inner api to add new learner data");
            var response = await client.PostWithResponseCode<object>(new PostLearnerDataRequest(command.LearnerData), false);
            if (!string.IsNullOrWhiteSpace(response.ErrorContent))
            {
                logger.LogInformation("Adding learner data returned status code {0} and error {1}", response.StatusCode, response.ErrorContent);
            }
            return ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred whilst calling inner api to add learner data");
            throw;
        }
    }
}