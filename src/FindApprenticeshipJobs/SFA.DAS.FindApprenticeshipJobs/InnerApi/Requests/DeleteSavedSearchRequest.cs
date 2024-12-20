using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public record DeleteSavedSearchRequest(Guid Id) : IDeleteApiRequest
    {
        public string DeleteUrl => $"api/SavedSearches/{Id}";
    }
}