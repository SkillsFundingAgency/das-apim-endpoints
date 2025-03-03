using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public record DeleteSavedSearchRequest(Guid Id) : IDeleteApiRequest
{
    public string DeleteUrl => $"api/SavedSearches/{Id}";
}