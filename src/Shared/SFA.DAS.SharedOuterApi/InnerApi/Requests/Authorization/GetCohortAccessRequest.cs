using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Authorization;

public record GetCohortAccessRequest(Party Party, long PartyId, long CohortId): IGetApiRequest
{
    public string GetUrl => "api/authorization/access-cohort";
}