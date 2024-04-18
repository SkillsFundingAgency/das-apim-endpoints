using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses
{
    public class PostGetVacanciesByReferenceApiResponse
    {
        public IEnumerable<ApprenticeshipVacancy> ApprenticeshipVacancies { get; set; }

        public class ApprenticeshipVacancy
        {
            public string VacancyReference { get; set; }
            public string EmployerName { get; set; }
            public string Title { get; set; }
            public DateTime ClosingDate { get; set; }
        }
    }
}