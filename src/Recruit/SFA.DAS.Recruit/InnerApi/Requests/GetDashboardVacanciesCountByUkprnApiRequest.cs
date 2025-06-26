using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetDashboardVacanciesCountByUkprnApiRequest(
        int Ukprn,
        int PageNumber = 1,
        int PageSize = 25,
        string SortColumn = "CreatedDate",
        bool IsAscending = false,
        ApplicationReviewStatus Status = ApplicationReviewStatus.New) : IGetApiRequest
    {
        public string GetUrl =>
            $"api/provider/{Ukprn}/applicationReviews/dashboard/vacancies?pageNumber={PageNumber}&pageSize={PageSize}&sortColumn={SortColumn}&isAscending={IsAscending}&status={Status}";
    }
}