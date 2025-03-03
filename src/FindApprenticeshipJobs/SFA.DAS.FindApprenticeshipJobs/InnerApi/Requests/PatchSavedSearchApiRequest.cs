using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public record PatchSavedSearchApiRequest : IPatchApiRequest<JsonPatchDocument<PatchSavedSearch>>
    {
        private readonly Guid _id;
        public JsonPatchDocument<PatchSavedSearch> Data { get; set; }

        public PatchSavedSearchApiRequest(
            Guid id,
            JsonPatchDocument<PatchSavedSearch> data)
        {
            _id = id;
            Data = data;
        }

        public string PatchUrl => $"api/savedSearches/{_id}";
    }
}