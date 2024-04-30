using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryResult
    {
        public GetApprenticeshipVacancyItemResponse ApprenticeshipVacancy { get; init; }
        public GetStandardsListItemResponse CourseDetail { get; init; }
        public List<GetCourseLevelsListItem> Levels { get; init; }
        public CandidateApplication? Application { get; set; } = null;

        public class CandidateApplication
        {
            public string Status { get; set; }
            public DateTime? SubmittedDate { get; set; }
        }
    }
}