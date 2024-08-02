using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Recruit.Application.Queries.GetAllVacanciesInMetrics;

namespace SFA.DAS.Recruit.Api.Models
{
    public record GetAllVacanciesInMetricsApiResponse
    {
        public List<long> Vacancies { get; set; } = [];

        public static implicit operator GetAllVacanciesInMetricsApiResponse(GetAllVacanciesInMetricsQueryResult source)
        {
            return new GetAllVacanciesInMetricsApiResponse
            {
                Vacancies = source.Vacancies.Select(c=>Convert.ToInt64(c.Replace("VAC","", StringComparison.CurrentCultureIgnoreCase))).Distinct().ToList()
            };
        }
    }
}
