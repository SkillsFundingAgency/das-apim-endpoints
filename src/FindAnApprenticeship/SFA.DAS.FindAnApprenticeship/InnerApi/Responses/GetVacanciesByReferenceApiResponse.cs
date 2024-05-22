using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Services;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses
{
    public class PostGetVacanciesByReferenceApiResponse
    {
        public IEnumerable<ApprenticeshipVacancy> ApprenticeshipVacancies { get; set; }

        public class ApprenticeshipVacancy : IVacancy
        {
            public string VacancyReference { get; set; }
            public string EmployerName { get; set; }
            public string Title { get; set; }
            public DateTime ClosingDate { get; set; }
            public DateTime? ClosedDate { get; set; }
            public int CourseId { get; set; }
            public string AdditionalQuestion1 { get; set; }
            public string AdditionalQuestion2 { get; set; }
            public bool IsDisabilityConfident { get; set; }
        }
    }
}