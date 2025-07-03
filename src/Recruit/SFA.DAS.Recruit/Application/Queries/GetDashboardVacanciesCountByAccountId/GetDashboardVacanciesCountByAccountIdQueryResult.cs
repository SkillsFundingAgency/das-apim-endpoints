using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByAccountId;
public record GetDashboardVacanciesCountByAccountIdQueryResult
{
    public int TotalCount { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }

    public List<VacancyCount> Items { get; init; } = [];

    public static implicit operator GetDashboardVacanciesCountByAccountIdQueryResult(GetDashboardVacanciesCountApiResponse source)
    {
        if ( source?.Info == null || source.Items == null)
        {
            return new GetDashboardVacanciesCountByAccountIdQueryResult
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
        return new GetDashboardVacanciesCountByAccountIdQueryResult
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
                Applications = i.Applications,
                Shared = i.Shared,
                AllSharedApplications = i.AllSharedApplications
            }).ToList()
        };
    }

    public record VacancyCount
    {
        public long VacancyReference { get; init; } = 0;
        public int NewApplications { get; init; } = 0;
        public int Applications { get; init; } = 0;
        public int Shared { get; set; } = 0;
        public int AllSharedApplications { get; set; } = 0;
    }
}