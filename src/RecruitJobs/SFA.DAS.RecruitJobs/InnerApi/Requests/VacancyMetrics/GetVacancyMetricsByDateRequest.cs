using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.VacancyMetrics;

public sealed record GetVacancyMetricsByDateRequest(DateTime StartDate, DateTime EndDate) : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            const string baseUrl = "api/vacancies/metrics";
            var queryParams = new Dictionary<string, string>
            {
                { "startDate", StartDate.ToString("s") },
                { "endDate", EndDate.ToString("s") }
            };

            return QueryHelpers.AddQueryString(baseUrl, queryParams);
        }
    }
}