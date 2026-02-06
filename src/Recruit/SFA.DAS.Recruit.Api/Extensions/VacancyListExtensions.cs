using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Recruit.Api.Models.Responses;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.Extensions;

public static class VacancyListExtensions
{
    public static List<VacancyListItem> AssignStatsToVacancies(this IReadOnlyList<IGetPagedVacanciesList_PagedVacancies_Items> items, Dictionary<long, VacancyStatsItem> statsData)
    {
        return items.Select(x =>
        {
            if (x.VacancyReference is not null && statsData.TryGetValue(x.VacancyReference!.Value, out var stats))
            {
                return VacancyListItem.From(x, stats);
            }
            
            return VacancyListItem.From(x, null);
        }).ToList();
    }
}