using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches
{
    public record GetSavedSearchesQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> FindApprenticeshipApiClient)
        : IRequestHandler<GetSavedSearchesQuery, GetSavedSearchesQueryResult>
    {
        public async Task<GetSavedSearchesQueryResult> Handle(GetSavedSearchesQuery request,
            CancellationToken cancellationToken)
        {
            var savedSearchResponse = await FindApprenticeshipApiClient.Get<GetSavedSearchesApiResponse>(
                new GetSavedSearchesApiRequest(request.LastRunDateFilter.ToString("O"),
                    request.PageNumber,
                    request.PageSize));

            return new GetSavedSearchesQueryResult
            {
                PageSize = savedSearchResponse.PageSize,
                PageIndex = savedSearchResponse.PageIndex,
                TotalPages = savedSearchResponse.TotalPages,
                TotalCount = savedSearchResponse.TotalCount,
                LastRunDateFilter = request.LastRunDateFilter,
                SavedSearchResults = savedSearchResponse
                    .SavedSearches
                    .Select(c=>(GetSavedSearchesQueryResult.SearchResult)c)
                    .ToList()
            };
        }
    }
}