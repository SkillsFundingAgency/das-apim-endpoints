using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public record DeleteSavedSearchesRequest(List<Guid> Ids) : IDeleteApiRequest
{
    public string DeleteUrl { get; } = QueryHelpers.AddQueryString("api/savedSearches", new Dictionary<string, StringValues>
    {
        { "ids", Ids.Select(x => $"{x}").ToArray() }
    });
}