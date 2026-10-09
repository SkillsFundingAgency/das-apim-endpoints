using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Commitments;

public sealed record GetPriceEpisodeByApprenticeshipIdRequest(long ApprenticeshipId) : IGetApiRequest
{
    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/price-episodes";
}