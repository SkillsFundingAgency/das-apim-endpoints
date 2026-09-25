using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests;

public record GetLevySummaryByAccountIdRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/levy-declarations/{AccountId}/summary";
}