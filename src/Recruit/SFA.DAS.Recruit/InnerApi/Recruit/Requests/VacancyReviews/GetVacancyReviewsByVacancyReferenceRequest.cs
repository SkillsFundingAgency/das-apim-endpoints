using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyReviews;

public class GetVacancyReviewsByVacancyReferenceRequest(long vacancyReference, string? status, bool includeNoStatus, List<string>? manualOutcome = null) : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            var url = $"api/vacancies/{vacancyReference}/Reviews?includeNoStatus={includeNoStatus}"; 
            
            if (!string.IsNullOrEmpty(status))
            {
                url += $"&status={status}";
            }
            if (manualOutcome != null && manualOutcome.Any())
            {
                url += $"&manualOutcome={string.Join("&manualOutcome=", manualOutcome)}";
            }

            return url;
        }
    }
}
