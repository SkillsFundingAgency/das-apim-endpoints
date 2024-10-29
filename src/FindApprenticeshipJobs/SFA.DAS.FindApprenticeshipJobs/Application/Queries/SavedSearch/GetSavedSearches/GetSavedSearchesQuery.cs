using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches
{
    public record GetSavedSearchesQuery(
        DateTime LastRunDateFilter,
        int PageNumber,
        int PageSize,
        int MaxApprenticeshipSearchResultsCount,
        VacancySort ApprenticeshipSearchResultsSortOrder) : IRequest<GetSavedSearchesQueryResult>;
}
