using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetPagedApplicationReviewsByVacancyReference;
public sealed record GetPagedApplicationReviewsByVacancyReferenceQueryResult
{
    public int TotalCount { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
    public List<Domain.ApplicationReview> Items { get; init; } = [];

    public static implicit operator GetPagedApplicationReviewsByVacancyReferenceQueryResult(InnerApi.Responses.GetPagedApplicationReviewsByVacancyReferenceApiResponse source)
    {
        if (source?.Info == null || source.Items == null)
        {
            return new GetPagedApplicationReviewsByVacancyReferenceQueryResult
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
        return new GetPagedApplicationReviewsByVacancyReferenceQueryResult
        {
            TotalCount = source.Info.TotalCount,
            PageIndex = source.Info.PageIndex,
            PageSize = source.Info.PageSize,
            TotalPages = source.Info.TotalPages,
            HasPreviousPage = source.Info.HasPreviousPage,
            HasNextPage = source.Info.HasNextPage,
            Items = source.Items
        };
    }
}
