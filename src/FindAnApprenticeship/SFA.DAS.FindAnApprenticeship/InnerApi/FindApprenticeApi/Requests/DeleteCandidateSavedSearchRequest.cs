using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public record DeleteCandidateSavedSearchRequest(Guid CandidateId, Guid Id) : IDeleteApiRequest
{
    public string DeleteUrl => $"api/Users/{CandidateId}/SavedSearches/{Id}";
}