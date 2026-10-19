using System.Globalization;
using System.Text.Json.Nodes;
using SFA.DAS.LearnerData.Requests.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;

namespace SFA.DAS.LearnerData.Services;

public interface IApprovalsRequestBuilder
{
    PutApprovalsApiRequest Build(long ukprn, long uln, BaseLearnerApiPutResponse learningApiPutResponse);
}

public class ApprovalsRequestBuilder : IApprovalsRequestBuilder
{
    public PutApprovalsApiRequest Build(long ukprn, long uln, BaseLearnerApiPutResponse learningApiPutResponse)
    {
        var body = new ApprovalsRequestBody
        {
            LearningKey = learningApiPutResponse.LearningKey,
            ApprenticeshipId = learningApiPutResponse.ApprovalsApprenticeshipId,
            LearningType = learningApiPutResponse.LearningType.ToString(),
            Ukprn = ukprn.ToString(CultureInfo.InvariantCulture),
            Uln = uln.ToString(CultureInfo.InvariantCulture)
        };

        var changesNeedingApproval = learningApiPutResponse.ChangeDetails.Where(x => x.RequiresApproval).ToList();

        var startDateChange = changesNeedingApproval.FirstOrDefault(x => x.ChangeType == BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        var pricesChanged = changesNeedingApproval.Any(x => x.ChangeType == BaseLearnerApiPutResponse.LearningUpdateChanges.Prices);

        // The full price structure is sent whenever prices, or the start date that is derived from them, changed.
        // Prices are signalled to Approvals by this structure alone, never as an item in Changes.
        if (startDateChange != null || pricesChanged)
        {
            body.NewPrices = learningApiPutResponse.Prices.Select(ToApprovalsPrice).ToList();
        }

        if (startDateChange != null)
        {
            body.Changes.Add(new ApprovalsFieldChange
            {
                ChangeType = ApprovalsChangeTypes.StartDate,
                Data = new ApprovalsChangeData
                {
                    // Old is Learning's last-held value, which can be stale and not useful.
                    Old = ToWireValue(startDateChange.Old),
                    New = ToWireValue(startDateChange.New)
                }
            });
        }

        return new PutApprovalsApiRequest(learningApiPutResponse.LearningKey, body);
    }

    // Scalars (dates are already yyyy-MM-dd strings) go to Approvals as strings
    private static string? ToWireValue(JsonNode? value) =>
        value is JsonValue jsonValue && jsonValue.TryGetValue<string>(out var text)
            ? text
            : value?.ToJsonString();

    private static ApprovalsPrice ToApprovalsPrice(BaseLearnerApiPutResponse.EpisodePrice price) => new()
    {
        TrainingPrice = price.TrainingPrice.ToString("F2", CultureInfo.InvariantCulture),
        AssessmentPrice = (price.EndPointAssessmentPrice ?? 0m).ToString("F2", CultureInfo.InvariantCulture),
        EffectiveFrom = price.StartDate
    };
}
