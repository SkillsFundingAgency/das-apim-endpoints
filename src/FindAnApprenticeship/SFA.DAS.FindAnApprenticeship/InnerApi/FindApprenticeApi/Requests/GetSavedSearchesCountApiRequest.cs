using System;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

public class GetSavedSearchesCountApiRequest(Guid candidateId) : IGetApiRequest
{
    public string GetUrl { get; } = $"api/Users/{candidateId}/SavedSearches/count";
}