using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Services;

public interface IFurtherApprovalService
{
    /// <summary>
    /// Handles the request for approval of a learning change where applicable.
    /// </summary>
    /// <returns>Whether FurtherApproval is still required, so Earnings knows whether to withhold payments</returns>
    Task<bool> Resolve(long ukprn, long uln, BaseLearnerApiPutResponse learningResponse);
}

public class FurtherApprovalService(
    ILogger<FurtherApprovalService> logger,
    ILearningApiClient<LearningApiConfiguration> learningApiClient,
    IApprovalsService approvalsService,
    IApprovedLearningUpdatedEventBuilder approvedLearningUpdatedEventBuilder,
    IMessageSession messageSession) : IFurtherApprovalService
{
    public async Task<bool> Resolve(long ukprn, long uln, BaseLearnerApiPutResponse learningResponse)
    {
        var cleared = false;
        if (learningResponse.ChangeDetails.Any(x => x.RequiresApproval))
        {
            var approval = await approvalsService.RequestApproval(ukprn, uln, learningResponse);
            if (approval.AllAutoApproved && await ClearFurtherApprovalNeeded(learningResponse))
            {
                cleared = true;
                await PublishApprovedLearningUpdatedEvent(learningResponse, approval.Response);
            }
        }

        // FurtherApprovalNeeded on the learning response is sourced from the episode's stored flag,
        // so this also covers a change still awaiting approval from an earlier update
        var furtherApprovalRequired = learningResponse.FurtherApprovalNeeded && !cleared;
        if (furtherApprovalRequired)
        {
            logger.LogInformation("Learning {LearningKey}, episode {EpisodeKey} needs further approval, so Earnings will withhold payments",
                learningResponse.LearningKey, learningResponse.LearningEpisodeKey);
        }

        return furtherApprovalRequired;
    }

    /// <summary>
    /// Clears the FurtherApprovalNeeded flag, catching and logging any failure so that Earnings will still withhold payments until the flag is cleared.
    /// </summary>
    private async Task<bool> ClearFurtherApprovalNeeded(BaseLearnerApiPutResponse learningResponse)
    {
        try
        {
            var response = await learningApiClient.PostWithResponseCode<object>(
                new ClearFurtherApprovalNeededApiPostRequest(learningResponse.LearningKey, learningResponse.LearningEpisodeKey, learningResponse.LearningType),
                false);

            if (!response.StatusCode.IsSuccessStatusCode())
            {
                logger.LogError("Change was auto-approved for learning {LearningKey}, episode {EpisodeKey}, but clearing FurtherApprovalNeeded failed with status {StatusCode}. The flag is still set",
                    learningResponse.LearningKey, learningResponse.LearningEpisodeKey, response.StatusCode);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Change was auto-approved for learning {LearningKey}, episode {EpisodeKey}, but clearing FurtherApprovalNeeded failed. The flag is still set",
                learningResponse.LearningKey, learningResponse.LearningEpisodeKey);
            return false;
        }
    }

    /// <summary>
    /// Echoes the approved changes back as an ApprovedLearningUpdatedEvent.
    /// Failures are logged and swallowed so that Earnings is still updated
    /// </summary>
    private async Task PublishApprovedLearningUpdatedEvent(BaseLearnerApiPutResponse learningResponse, ApprovalsResponse approvalsResponse)
    {
        // Nb, this event belongs to Learning: it describes a change to approved learning. It is published here, by agreed
        // design, because only the outer API holds the Approvals response at this point. A change approved later by the
        // employer never passes through this service, so it will need publishing from elsewhere.
        // The event itself is also dishonest: it reads as Learning reporting what changed, but its changes and their old and
        // new values are copied from Approvals' response and sent straight back to Approvals. It is defined as an event, but is a
        // command in disguise, since it is really an instruction to Approvals to apply the change it has just auto-approved.

        try
        {
            await messageSession.Publish(approvedLearningUpdatedEventBuilder.Build(learningResponse, approvalsResponse));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Change was auto-approved for learning {LearningKey} (apprenticeship {ApprenticeshipId}), but publishing ApprovedLearningUpdatedEvent failed. The event has not been sent for changes: {Changes}",
                learningResponse.LearningKey, learningResponse.ApprovalsApprenticeshipId, string.Join(", ", approvalsResponse.Changes.Select(x => x.ChangeType)));
        }
    }
}
