using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;

public class GetApprenticeshipRequest(long apprenticeshipId) : IGetApiRequest
{
    public readonly long ApprenticeshipId = apprenticeshipId;

    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}";
}