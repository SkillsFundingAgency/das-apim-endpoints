using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetVacanciesByAccountId;
public record GetVacanciesByAccountIdQueryResult
{
    public GetPagedVacancySummaryApiResponse.Info PageInfo { get; set; }
    public List<VacancySummary> VacancySummaries { get; set; }

    public static GetVacanciesByAccountIdQueryResult FromResponses(
        GetPagedVacancySummaryApiResponse source)
    {
        if (source is null) return new GetVacanciesByAccountIdQueryResult
        {
            PageInfo = new GetPagedVacancySummaryApiResponse.Info(),
            VacancySummaries = []
        };

        return new GetVacanciesByAccountIdQueryResult
        {
            PageInfo = source.PageInfo,
            VacancySummaries = source.Items,
        };
    }
}