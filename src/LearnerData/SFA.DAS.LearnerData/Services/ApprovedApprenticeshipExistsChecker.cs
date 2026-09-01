using System.Net;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Services;

public interface IApprovedApprenticeshipExistsChecker
{
    Task<bool> Exists(long ukprn, string uln, int standardCode, DateTime startDate);
}

public class ApprovedApprenticeshipExistsChecker(ILearningApiClient<LearningApiConfiguration> learningApiClient)
    : IApprovedApprenticeshipExistsChecker
{
    public async Task<bool> Exists(long ukprn, string uln, int standardCode, DateTime startDate)
    {
        var request = new CheckApprovedApprenticeshipExistsRequest
        {
            Ukprn = ukprn,
            Uln = uln,
            TrainingCode = standardCode.ToString(),
            StartDate = startDate,
            IsApproved = true
        };

        var statusCode = await learningApiClient.Head(request);

        return statusCode == HttpStatusCode.OK;
    }
}
