using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByUkprn;
public record GetDashboardVacanciesCountByUkprnQueryResult
{
    public int TotalCount { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }

    public List<VacancyCount> Items { get; init; } = [];

    public static implicit operator GetDashboardVacanciesCountByUkprnQueryResult(GetDashboardVacanciesCountApiResponse source)
    {
        if (source?.Info == null || source.Items == null)
        {
            return new GetDashboardVacanciesCountByUkprnQueryResult
            {
                TotalCount = 0,
                PageIndex = 0,
                PageSize = 0,
                TotalPages = 0,
                HasPreviousPage = false,
                HasNextPage = false,
                Items = []
            };
        }
        return new GetDashboardVacanciesCountByUkprnQueryResult
        {
            TotalCount = source.Info.TotalCount,
            PageIndex = source.Info.PageIndex,
            PageSize = source.Info.PageSize,
            TotalPages = source.Info.TotalPages,
            HasPreviousPage = source.Info.HasPreviousPage,
            HasNextPage = source.Info.HasNextPage,
            Items = source.Items.Select(i => new VacancyCount
            {
                VacancyReference = i.VacancyReference,
                NewApplications = i.NewApplications,
                Applications = i.Applications
            }).ToList()
        };
    }

    public record VacancyCount
    {
        public long VacancyReference { get; init; }
        public int NewApplications { get; init; }
        public int Applications { get; init; }
    }
}