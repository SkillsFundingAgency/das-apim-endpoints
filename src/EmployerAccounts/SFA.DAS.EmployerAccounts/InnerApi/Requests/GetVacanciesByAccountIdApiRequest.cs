using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.InnerApi.Requests;

public record GetVacanciesByAccountIdApiRequest(
    long AccountId,
    int Page = 1,
    int PageSize = 1) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{AccountId}/vacancies?page={Page}&pageSize={PageSize}&sortColumn=&sortOrder=Desc&filterBy=All&searchTerm=";
}