using System.Collections.Generic;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetDashboardVacanciesCountByAccountIdApiRequest(
        long AccountId,
        int PageNumber = 1,
        int PageSize = 25,
        string SortColumn = "CreatedDate",
        bool IsAscending = false,
        List<ApplicationReviewStatus>? Status = null) : IGetApiRequest
    {
        public string GetUrl =>
            $"api/employer/{AccountId}/applicationReviews/dashboard/vacancies?pageNumber={PageNumber}&pageSize={PageSize}&sortColumn={SortColumn}&isAscending={IsAscending}&status={string.Join("&status=",Status) }";
    }
}