using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;

public class GetApprenticeshipRequest(long apprenticeshipId) : IGetApiRequest
{
    public readonly long ApprenticeshipId = apprenticeshipId;

    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}";
}