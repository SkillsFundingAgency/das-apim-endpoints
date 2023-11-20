using SFA.DAS.Vacancies.Api.Models;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

public class GetVacanciesResponse
{
    public List<GetVacanciesListResponse> Vacancies { get; set; }
}