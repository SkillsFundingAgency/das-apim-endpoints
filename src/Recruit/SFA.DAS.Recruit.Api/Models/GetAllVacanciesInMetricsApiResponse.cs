using System.Collections.Generic;
using SFA.DAS.Recruit.Application.Queries.GetAllVacanciesInMetrics;

namespace SFA.DAS.Recruit.Api.Models
{
    public record GetAllVacanciesInMetricsApiResponse
    {
        public List<string> Vacancies { get; set; } = [];

        public static implicit operator GetAllVacanciesInMetricsApiResponse(GetAllVacanciesInMetricsQueryResult source)
        {
            return new GetAllVacanciesInMetricsApiResponse
            {
                Vacancies = source.Vacancies
            };
        }
    }
}
