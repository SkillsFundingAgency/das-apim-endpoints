using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Authorization;

public record GetCohortAccessRequest(Party Party, long PartyId, long CohortId): IGetApiRequest
{
    public string GetUrl => $"api/authorization/access-cohort?party={(int)Party}&partyId={PartyId}&cohortId={CohortId}";
}