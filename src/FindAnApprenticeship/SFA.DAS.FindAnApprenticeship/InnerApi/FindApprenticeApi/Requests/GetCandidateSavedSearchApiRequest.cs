using System;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public record GetCandidateSavedSearchApiRequest(Guid CandidateId, Guid Id) : IGetApiRequest
{
    public string GetUrl => $"api/Users/{CandidateId}/SavedSearches/{Id}";
}
