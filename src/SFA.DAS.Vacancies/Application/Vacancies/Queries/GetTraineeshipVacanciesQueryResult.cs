﻿using System.Collections.Generic;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetTraineeshipVacanciesQueryResult
    {
        public IEnumerable<GetTraineeshipVacanciesListItem> Vacancies { get; set; }
        public long Total { get; set; }
        public long TotalFiltered { get; set; }
        public int TotalPages { get; set; }
    }
}
