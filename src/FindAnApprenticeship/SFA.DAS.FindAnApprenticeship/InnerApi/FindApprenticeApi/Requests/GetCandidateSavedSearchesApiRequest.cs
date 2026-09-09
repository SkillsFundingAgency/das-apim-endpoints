using System;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public record GetCandidateSavedSearchesApiRequest(Guid CandidateId) : IGetApiRequest
{
    public string GetUrl => $"api/Users/{CandidateId}/SavedSearches";
}
