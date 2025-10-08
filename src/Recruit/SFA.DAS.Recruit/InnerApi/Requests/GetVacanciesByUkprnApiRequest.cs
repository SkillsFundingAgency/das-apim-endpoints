using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetVacanciesByUkprnApiRequest(
    int Ukprn,
    int Page = 1,
    int PageSize = 25,
    string SortColumn = "",
    string SortOrder = "Desc",
    FilteringOptions FilterBy = FilteringOptions.All,
    string SearchTerm = "") : IGetApiRequest
{
    public string GetUrl => $"api/provider/{Ukprn}/vacancies?page={Page}&pageSize={PageSize}&sortColumn={SortColumn}&sortOrder={SortOrder}&filterBy={FilterBy}&searchTerm={SearchTerm}";
}
