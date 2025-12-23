using System.Collections.Generic;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerVacancies;

public class GetEmployerVacanciesResponse
{
    public List<VacancySummary> Vacancies { get; set; }
}