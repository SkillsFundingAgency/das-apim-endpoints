using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryResult
{
    public IEnumerable<LiveVacancy> Vacancies { get; set; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }
}

