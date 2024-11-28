using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;

public class GetCandidateSavedSearchesApiResponse
{
    public List<SavedSearchDto> SavedSearches { get; set; }
}