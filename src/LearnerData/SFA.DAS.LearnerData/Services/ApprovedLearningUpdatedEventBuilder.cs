using SFA.DAS.LearnerData.Requests.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.Learning.Types;

namespace SFA.DAS.LearnerData.Services;

public interface IApprovedLearningUpdatedEventBuilder
{
    ApprovedLearningUpdatedEvent Build(BaseLearnerApiPutResponse learningApiPutResponse, ApprovalsResponse approvalsResponse);
}

/// <summary>
/// The event echoes back to Approvals the changes it auto-approved, so that it applies them. Field changes are copied from
/// Approvals' response. Auto-approved prices are sent as the price list from our own response, which is the list we asked about.
/// </summary>
public class ApprovedLearningUpdatedEventBuilder : IApprovedLearningUpdatedEventBuilder
{
    public ApprovedLearningUpdatedEvent Build(BaseLearnerApiPutResponse learningApiPutResponse, ApprovalsResponse approvalsResponse)
    {
        var autoApproved = approvalsResponse.Changes.Where(x => x.IsAutoApproved).ToList();
        var pricesAutoApproved = autoApproved.Any(IsPrices);

        return new ApprovedLearningUpdatedEvent
        {
            LearningKey = learningApiPutResponse.LearningKey,
            ApprenticeshipId = learningApiPutResponse.ApprovalsApprenticeshipId,
            LearningType = learningApiPutResponse.LearningType.ToString(),
            Changes = autoApproved
                .Where(x => !IsPrices(x))
                .Select(x => new ApprenticeshipFieldChange
                {
                    ChangeType = x.ChangeType,
                    Data = new ApprenticeshipData
                    {
                        Old = x.Data?.Old ?? string.Empty,
                        New = x.Data?.New ?? string.Empty
                    }
                })
                .ToList(),
            Prices = pricesAutoApproved
                ? learningApiPutResponse.Prices
                    .Select(x => new PriceEpisode
                    {
                        EffectiveFrom = x.StartDate,
                        TrainingPrice = x.TrainingPrice,
                        AssessmentPrice = x.EndPointAssessmentPrice ?? 0
                    })
                    .ToList()
                : []
        };
    }

    private static bool IsPrices(ApprovalsChangeResult change) =>
        string.Equals(change.ChangeType, nameof(CocChangeField.Prices), StringComparison.OrdinalIgnoreCase);
}
