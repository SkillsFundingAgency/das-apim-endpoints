using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class GetSavedVacanciesApiResponse
    {
        public List<SavedVacancy> SavedVacancies { get; set; } = [];

        public class SavedVacancy
        {
            public Guid Id { get; set; }
            public Guid CandidateId { get; set; }
            public string VacancyReference { get; set; }
            public string VacancyId { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}
