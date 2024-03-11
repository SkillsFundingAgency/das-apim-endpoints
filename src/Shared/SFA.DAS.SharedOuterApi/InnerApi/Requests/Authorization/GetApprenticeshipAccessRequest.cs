using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Authorization;

public record GetApprenticeshipAccessRequest(Party Party, long PartyId, long ApprenticeshipId) : IGetApiRequest
{
    public string GetUrl => $"api/authorization/access-apprenticeship?Party={(int)Party}&PartyId={PartyId}&ApprenticeshipId={ApprenticeshipId}";
}