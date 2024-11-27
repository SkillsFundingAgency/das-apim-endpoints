using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public record DeleteSavedSearchesApiRequest(Guid CandidateId) : IDeleteApiRequest
{
    public string DeleteUrl => $"api/Users/{CandidateId}/SavedSearches";
}