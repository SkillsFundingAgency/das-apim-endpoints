using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches
{
    public record GetSavedSearchesQuery(
        DateTime LastRunDateFilter,
        int PageNumber = 1,
        int PageSize = 10) : IRequest<GetSavedSearchesQueryResult>;
}
