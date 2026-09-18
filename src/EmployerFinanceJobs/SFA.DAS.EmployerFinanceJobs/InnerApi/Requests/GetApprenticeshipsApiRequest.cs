using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetApprenticeshipsApiRequest(long ApprenticeshipId) : IGetApiRequest
{
    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}";
}