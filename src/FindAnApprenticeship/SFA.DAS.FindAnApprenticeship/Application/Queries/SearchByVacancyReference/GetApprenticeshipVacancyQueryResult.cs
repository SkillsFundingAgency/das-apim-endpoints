using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryResult
    {
        public GetApprenticeshipVacancyItemResponse ApprenticeshipVacancy { get; set; }
        public GetStandardsListItemResponse CourseDetail { get; set; }
    }
}
