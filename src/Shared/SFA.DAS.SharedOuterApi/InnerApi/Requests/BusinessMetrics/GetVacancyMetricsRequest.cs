using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.BusinessMetrics
{
    public record GetVacancyMetricsRequest(
        string ServiceName,
        string VacancyReference,
        DateTime StartDateTime,
        DateTime EndDateTime) : IGetApiRequest
    {
        public string GetUrl => $"api/vacancy/{ServiceName}/metrics/{VacancyReference}?startDate={StartDateTime:O}&endDate={EndDateTime:O}";
    }
}
