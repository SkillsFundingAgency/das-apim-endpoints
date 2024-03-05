using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Authorization;

public record GetCohortAccessRequest(Party Party, long PartyId, long CohortId): IGetApiRequest
{
    public string GetUrl => "api/authorization/access-cohort";
}