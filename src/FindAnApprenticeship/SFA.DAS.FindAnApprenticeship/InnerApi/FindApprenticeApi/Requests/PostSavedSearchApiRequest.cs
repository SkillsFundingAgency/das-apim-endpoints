using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public record PostSavedSearchApiRequestData(Guid UserReference, string SearchParameters);

public class PostSavedSearchApiRequest(PostSavedSearchApiRequestData payload) : IPostApiRequest
{
    public object Data { get; set; } = payload;

    public string PostUrl => $"/api/SavedSearches";
}