using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public record PutSavedSearchApiRequestData(SaveSearchRequest SaveSearchRequest);

public class PutSavedSearchApiRequest(Guid candidateId, Guid id, PutSavedSearchApiRequestData payload) : IPutApiRequest
{
    public string PutUrl => $"/api/Users/{candidateId}/SavedSearches/{id}";
    public object Data { get; set; } = payload;
}

public class SaveSearchRequest
{
    public string UnSubscribeToken { get; set; }
    public SearchParameters SearchParameters { get; init; }
}