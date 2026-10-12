using System.Text.Json.Nodes;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.Learning.Types;

namespace SFA.DAS.LearnerData.Services;

public interface IApprovedLearningUpdatedEventBuilder
{
    ApprovedLearningUpdatedEvent Build(BaseLearnerApiPutResponse learningApiPutResponse);
}

public class ApprovedLearningUpdatedEventBuilder : IApprovedLearningUpdatedEventBuilder
{
    private const string StartDate = nameof(ApprovedLearningUpdatedChangeType.StartDate);

    public ApprovedLearningUpdatedEvent Build(BaseLearnerApiPutResponse learningApiPutResponse)
    {
        return new ApprovedLearningUpdatedEvent
        {
            LearningKey = learningApiPutResponse.LearningKey,
            ApprenticeshipId = learningApiPutResponse.ApprovalsApprenticeshipId,
            LearningType = learningApiPutResponse.LearningType.ToString().ToLowerInvariant(),
            Changes = learningApiPutResponse.ChangeDetails
                .Where(x => x.RequiresApproval && x.ChangeType == BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate)
                .Select(x => new ApprenticeshipFieldChange
                {
                    ChangeType = StartDate,
                    Data = new ApprenticeshipData
                    {
                        Old = ToWireValue(x.Old),
                        New = ToWireValue(x.New)
                    }
                })
                .ToList()
        };
    }

    // Scalars (dates are already yyyy-MM-dd strings) go to Approvals as strings
    private static string ToWireValue(JsonNode? value) =>
        value is JsonValue jsonValue && jsonValue.TryGetValue<string>(out var text)
            ? text
            : value?.ToJsonString() ?? string.Empty;
}
