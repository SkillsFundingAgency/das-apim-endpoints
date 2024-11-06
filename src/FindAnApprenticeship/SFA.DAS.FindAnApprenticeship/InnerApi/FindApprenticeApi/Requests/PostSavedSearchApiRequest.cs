using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public record PostSavedSearchApiRequestData(Guid UserReference, SearchParameters SearchParameters);

public class PostSavedSearchApiRequest(PostSavedSearchApiRequestData payload) : IPostApiRequest
{
    public object Data { get; set; } = payload;

    public string PostUrl => $"/api/SavedSearches";
}