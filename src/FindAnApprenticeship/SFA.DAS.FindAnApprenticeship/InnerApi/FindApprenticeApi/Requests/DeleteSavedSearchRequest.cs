using System;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public record DeleteSavedSearchRequest(Guid Id) : IDeleteApiRequest
{
    public string DeleteUrl => $"api/SavedSearches/{Id}";
}