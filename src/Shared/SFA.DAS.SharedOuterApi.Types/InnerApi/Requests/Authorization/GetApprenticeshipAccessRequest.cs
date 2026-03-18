using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Authorization;

public record GetApprenticeshipAccessRequest(Party Party, long PartyId, long ApprenticeshipId) : IGetApiRequest
{
    public string GetUrl => $"api/authorization/access-apprenticeship?Party={(int)Party}&PartyId={PartyId}&ApprenticeshipId={ApprenticeshipId}";
}