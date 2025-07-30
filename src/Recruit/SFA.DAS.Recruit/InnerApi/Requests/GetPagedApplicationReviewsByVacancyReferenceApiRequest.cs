using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetPagedApplicationReviewsByVacancyReferenceApiRequest(long VacancyReference, 
    int PageNumber = 1, 
    int PageSize = 10, 
    string SortColumn = "CreatedDate", 
    bool IsAscending = false) : IGetApiRequest
{
    public string GetUrl => $"api/applicationreviews/paginated/{VacancyReference}?pageNumber={PageNumber}&pageSize={PageSize}&sortColumn={SortColumn}&isAscending={IsAscending}";
}