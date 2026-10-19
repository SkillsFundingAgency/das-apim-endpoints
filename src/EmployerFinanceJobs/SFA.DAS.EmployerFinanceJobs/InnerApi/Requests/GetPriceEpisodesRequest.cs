using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetPriceEpisodesRequest(long ApprenticeshipId) : IGetApiRequest
{
    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/price-episodes";
}