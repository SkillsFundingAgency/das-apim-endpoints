using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.Application.Handlers;

public class AddLearnerDataCommandHandler(IInternalApiClient<LearnerDataInnerApiConfiguration> client, ILogger<AddLearnerDataCommandHandler> logger)
    : IRequestHandler<AddLearnerDataCommand, bool>
{
    public async Task<bool> Handle(AddLearnerDataCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Building PUT request to add new learner data");
            var request = new PutLearnerDataRequest(command.LearnerData.UKPRN, command.LearnerData.ULN,
                command.LearnerData.AcademicYear, command.LearnerData.StandardCode, command.LearnerData);

            logger.LogInformation("Calling inner api to add new learner data");
            var response = await client.PutWithResponseCode<NullResponse>(request);
            if (!string.IsNullOrWhiteSpace(response.ErrorContent))
            {
                logger.LogInformation("Adding learner data returned status code {0} and error {1}", response.StatusCode, response.ErrorContent);
            }
            return (int)response.StatusCode >= 200 && (int)response.StatusCode <= 299;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred whilst calling inner api to add learner data");
            throw;
        }
    }
}