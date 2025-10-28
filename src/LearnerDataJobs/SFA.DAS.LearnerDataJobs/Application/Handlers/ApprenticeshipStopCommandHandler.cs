using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.Application.Handlers;
public class ApprenticeshipStopCommandHandler(IInternalApiClient<LearnerDataInnerApiConfiguration> client, ILogger<ApprenticeshipStopCommand> logger)
: IRequestHandler<ApprenticeshipStopCommand, bool>
{

    public async Task<bool> Handle(ApprenticeshipStopCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace($"Handling ApprenticeshipStopCommand");
            var getLearnerrequest = new GetLearnerByIdRequest(command.ProviderId, command.LearnerDataId);

            var learner = await client.GetWithResponseCode<GetLearnerDataByIdResponse>(getLearnerrequest);                

            if (learner == null)
            {
                logger.LogTrace($"Learner Data  does not exists for Id : {command.LearnerDataId}");
                return false;
            }
            
            if (command.PatchRequest.ApprenticeshipId != 0 && command.PatchRequest.ApprenticeshipId == learner.Body.ApprenticeshipId && command.PatchRequest.IsWithDrawnAtStartOfCourse)
            {                
                var request = new PatchLearnerDataApprenticeshipIdRequest(command.ProviderId, command.LearnerDataId, new LearnerDataApprenticeshipIdRequest() { ApprenticeshipId = null });                
                
                var response = await client.PatchWithResponseCode(request);

                if (!string.IsNullOrWhiteSpace(response.ErrorContent))
                {
                    logger.LogInformation("Assigning ApprenticeshipId returned status code {0} and error {1}", response.StatusCode, response.ErrorContent);
                }
                return (int)response.StatusCode >= 200 && (int)response.StatusCode <= 299;
            }

            logger.LogInformation($"ApprenticeshipStopCommand not successful for LearnerDataId;{command.LearnerDataId}");
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred whilst calling inner api to assign ApprenticeshipId");
            throw;
        }
    }
}
