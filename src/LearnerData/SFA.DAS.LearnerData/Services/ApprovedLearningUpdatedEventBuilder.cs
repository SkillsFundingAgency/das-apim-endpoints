using SFA.DAS.LearnerData.Responses.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.Learning.Types;

namespace SFA.DAS.LearnerData.Services;

public interface IApprovedLearningUpdatedEventBuilder
{
    ApprovedLearningUpdatedEvent Build(BaseLearnerApiPutResponse learningApiPutResponse, ApprovalsResponse approvalsResponse);
}

/// <summary>
/// The event is an echo of what Approvals responded with: every change it returned is passed back, with its old and
/// new values, under the change type name Approvals used. Only the identity of the learning comes from our own response.
/// </summary>
public class ApprovedLearningUpdatedEventBuilder : IApprovedLearningUpdatedEventBuilder
{
    public ApprovedLearningUpdatedEvent Build(BaseLearnerApiPutResponse learningApiPutResponse, ApprovalsResponse approvalsResponse)
    {
        return new ApprovedLearningUpdatedEvent
        {
            LearningKey = learningApiPutResponse.LearningKey,
            ApprenticeshipId = learningApiPutResponse.ApprovalsApprenticeshipId,
            LearningType = learningApiPutResponse.LearningType.ToString(),
            Changes = approvalsResponse.Changes
                .Select(x => new ApprenticeshipFieldChange
                {
                    ChangeType = x.ChangeType,
                    Data = new ApprenticeshipData
                    {
                        Old = x.Data?.Old ?? string.Empty,
                        New = x.Data?.New ?? string.Empty
                    }
                })
                .ToList()
        };
    }
}
