using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.BusinessMetrics
{
    public record GetVacancyMetricsRequest(
        DateTime StartDateTime,
        DateTime EndDateTime) : IGetApiRequest
    {
        public string GetUrl => $"api/vacancies/metrics?startDate={StartDateTime:O}&endDate={EndDateTime:O}";
    }
}
