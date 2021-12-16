using System.Collections.Generic;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetVacanciesQueryResult
    {
        public IEnumerable<GetVacanciesItem> Vacancies { get; set; }
    }
}
