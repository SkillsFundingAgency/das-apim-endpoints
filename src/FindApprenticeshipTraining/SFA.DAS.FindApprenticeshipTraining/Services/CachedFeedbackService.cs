using System;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Services;

public interface ICachedFeedbackService
{
    Task<(EmployerFeedbackAnnualDetails, ApprenticeFeedbackAnnualDetails)> GetProviderFeedback(long ukprn);
}

public class CachedFeedbackService(IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient,
        IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient, ICacheStorageService _cacheStorageService) : ICachedFeedbackService
{
    public async Task<(EmployerFeedbackAnnualDetails, ApprenticeFeedbackAnnualDetails)> GetProviderFeedback(long ukprn)
    {
        var empFbCacheKey = $"{nameof(EmployerFeedbackAnnualDetails)}-{ukprn}";
        var employerFB = await _cacheStorageService.RetrieveFromCache<EmployerFeedbackAnnualDetails>(empFbCacheKey);

        if (employerFB == null)
        {
            var employerFeedbackResponse = await _employerFeedbackApiClient.GetWithResponseCode<EmployerFeedbackAnnualDetails>(
            new GetEmployerFeedbackSummaryAnnualRequest(ukprn));

            if (employerFeedbackResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                employerFB = employerFeedbackResponse.Body;
                await _cacheStorageService.SaveToCache(
                   empFbCacheKey,
                    employerFB,
                    TimeSpan.FromHours(4));
            }
        }

        var appFBCacheKey = $"{nameof(ApprenticeFeedbackAnnualDetails)}-{ukprn}";
        var apprenticeFB = await _cacheStorageService.RetrieveFromCache<ApprenticeFeedbackAnnualDetails>(appFBCacheKey);
        if (apprenticeFB == null)
        {
            var apprenticeFeedbackResponse = await _apprenticeFeedbackApiClient.GetWithResponseCode<ApprenticeFeedbackAnnualDetails>(
                new GetApprenticeFeedbackSummaryAnnualRequest(ukprn));
            if (apprenticeFeedbackResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                apprenticeFB = apprenticeFeedbackResponse.Body;
                await _cacheStorageService.SaveToCache(
                    appFBCacheKey,
                    apprenticeFB,
                    TimeSpan.FromHours(4));
            }
        }

        return (employerFB, apprenticeFB);
    }
}
