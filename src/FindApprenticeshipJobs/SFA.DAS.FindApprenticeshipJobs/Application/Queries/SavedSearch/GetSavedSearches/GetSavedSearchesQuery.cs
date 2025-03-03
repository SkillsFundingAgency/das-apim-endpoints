using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches
{
    public record GetSavedSearchesQuery(
        DateTime LastRunDateFilter,
        int PageNumber = 1,
        int PageSize = 10,
        int MaxApprenticeshipSearchResultsCount = 5,
        VacancySort ApprenticeshipSearchResultsSortOrder = VacancySort.AgeDesc) : IRequest<GetSavedSearchesQueryResult>;
}
