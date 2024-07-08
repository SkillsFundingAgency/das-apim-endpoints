using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetAllVacanciesInMetrics
{
    public record GetAllVacanciesInMetricsQueryResult
    {
        public List<string> Vacancies { get; set; }
    }
}
