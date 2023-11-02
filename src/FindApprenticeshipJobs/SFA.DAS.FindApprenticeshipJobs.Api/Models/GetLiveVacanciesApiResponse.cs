using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Models;

public class GetLiveVacanciesApiResponse
{
    public static implicit operator GetLiveVacanciesApiResponse(GetLiveVacanciesQueryResult source)
    {
        return new GetLiveVacanciesApiResponse
        {
            Vacancies = source.Vacancies,
            PageSize = source.PageSize,
            PageNo = source.PageNo,
            TotalLiveVacanciesReturned = source.TotalLiveVacanciesReturned,
            TotalLiveVacancies = source.TotalLiveVacancies,
            TotalPages = source.TotalPages
        };
    }

    public IEnumerable<LiveVacancy> Vacancies { get; set; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }
}
