using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetVacanciesByUkprn;
public record GetVacanciesByUkprnQueryResult
{
    public GetPagedVacancySummaryApiResponse.Info PageInfo { get; set; }
    public List<VacancySummary> VacancySummaries { get; set; }

    public static GetVacanciesByUkprnQueryResult FromResponses(
        GetPagedVacancySummaryApiResponse source)
    {
        if (source is null) return new GetVacanciesByUkprnQueryResult
        {
            PageInfo = new GetPagedVacancySummaryApiResponse.Info(),
            VacancySummaries = []
        };

        return new GetVacanciesByUkprnQueryResult
        {
            PageInfo = source.PageInfo,
            VacancySummaries = source.Items,
        };
    }
}