using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public record GetSavedSearchesApiRequest(string LastRunDateTime, int PageNumber, int PageSize) : IGetApiRequest
    {
        public string GetUrl => $"api/savedSearches?lastRunDateFilter={LastRunDateTime}&pageNumber={PageNumber}&pageSize={PageSize}";
    }
}