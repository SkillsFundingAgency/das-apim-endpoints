using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.Application.Commands;

public class AddLearnerDataCommandHandler(IInternalApiClient<LearnerDataInnerApiConfiguration> client, ILogger<AddLearnerDataCommandHandler> logger)
    : IRequestHandler<AddLearnerDataCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(AddLearnerDataCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace("Calling inner api to add new learner data");
            var response = await client.PostWithResponseCode<object>(new PostLearnerDataRequest(command.LearnerData), false);
            return response.StatusCode;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred whilst adding learner data");
            throw;
        }
    }
}