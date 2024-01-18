using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryResult
    {
        public GetApprenticeshipVacancyItemResponse ApprenticeshipVacancy { get; init; }
        public GetStandardsListItemResponse CourseDetail { get; init; }
        public List<GetLevelsListItem> Levels { get; set; }
    }
}