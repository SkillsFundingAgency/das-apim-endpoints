using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;

public record GetCohortAccessRequest(long ProviderId, long CohortId): IGetApiRequest
{
    public string GetUrl => $"api/authorization/access-cohort?party={(int)Party.Provider}&partyId={ProviderId}&cohortId={CohortId}";
}