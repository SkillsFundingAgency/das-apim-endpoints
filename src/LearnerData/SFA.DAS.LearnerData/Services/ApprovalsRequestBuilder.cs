using System.Globalization;
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

        var startDateChanged = learningApiPutResponse.ChangesNeedingApproval.Contains(BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        var pricesChanged = learningApiPutResponse.ChangesNeedingApproval.Contains(BaseLearnerApiPutResponse.LearningUpdateChanges.Prices);

        // The full price structure is sent whenever prices, or the start date that is derived from them, changed
        if (startDateChanged || pricesChanged)
        {
            body.NewPrices = learningApiPutResponse.Prices.Select(ToApprovalsPrice).ToList();
        }

        if (startDateChanged)
        {
            body.Changes.Add(new ApprovalsFieldChange
            {
                ChangeType = ApprovalsChangeTypes.StartDate,
                Data = new ApprovalsChangeData
                {
                    // Old is deliberately not sent: Approvals holds the approved record, and learning does not
                    Old = null,
                    New = learningApiPutResponse.Prices.Min(x => x.StartDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                }
            });
        }

        return new PutApprovalsApiRequest(learningApiPutResponse.LearningKey, body);
    }

    private static ApprovalsPrice ToApprovalsPrice(BaseLearnerApiPutResponse.EpisodePrice price) => new()
    {
        TrainingPrice = price.TrainingPrice.ToString("F2", CultureInfo.InvariantCulture),
        AssessmentPrice = (price.EndPointAssessmentPrice ?? 0m).ToString("F2", CultureInfo.InvariantCulture),
        EffectiveFrom = price.StartDate
    };
}
