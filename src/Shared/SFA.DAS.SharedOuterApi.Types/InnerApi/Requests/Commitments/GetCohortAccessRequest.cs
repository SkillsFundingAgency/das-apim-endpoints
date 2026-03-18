using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;

public record GetCohortAccessRequest(long ProviderId, long CohortId): IGetApiRequest
{
    public string GetUrl => $"api/authorization/access-cohort?party={(int)Party.Provider}&partyId={ProviderId}&cohortId={CohortId}";
}