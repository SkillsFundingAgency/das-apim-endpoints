using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Authorization;

public record GetApprenticeshipAccessRequest(Party Party, long PartyId, long ApprenticeshipId) : IGetApiRequest
{
    public string GetUrl => "api/authorization/access-apprenticeship";
}