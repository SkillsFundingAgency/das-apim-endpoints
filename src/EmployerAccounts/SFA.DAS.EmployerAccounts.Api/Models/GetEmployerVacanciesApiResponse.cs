using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerVacancies;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;

namespace SFA.DAS.EmployerAccounts.Api.Models;

public class GetEmployerVacanciesApiResponse
{
    public List<VacancyApiResponse> Vacancies { get; set; }
    
    public static implicit operator GetEmployerVacanciesApiResponse(GetEmployerVacanciesQueryResponse source)
    {
        return new GetEmployerVacanciesApiResponse
        {
            Vacancies = source.Vacancies.Select(c => (VacancyApiResponse)c).ToList()
        };
    }
}

public class VacancyApiResponse
{
    public string Title { get; set; }
    public string Status { get; set; }
    public int? NoOfNewApplications { get; set; }
    public int? NoOfSuccessfulApplications { get; set; }
    public int? NoOfUnsuccessfulApplications { get; set; }
    public DateTime? ClosingDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    
    public static implicit operator VacancyApiResponse(VacancySummary source)
    {
        return new VacancyApiResponse
        {
            Title = source.Title,
            Status = source.Status.ToString(),
            NoOfNewApplications = source.NoOfNewApplications,
            NoOfSuccessfulApplications = source.NoOfSuccessfulApplications,
            NoOfUnsuccessfulApplications = source.NoOfUnsuccessfulApplications,
            ClosingDate = source.ClosingDate,
            ClosedDate = source.ClosedDate
        };
    }
}