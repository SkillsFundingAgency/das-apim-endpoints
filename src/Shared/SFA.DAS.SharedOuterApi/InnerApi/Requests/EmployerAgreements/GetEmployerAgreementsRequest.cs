using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAgreements;

public record GetEmployerAgreementsRequest(long accountId) : IGetAllApiRequest 
{
    public string GetAllUrl => $"api/accounts/{accountId}/agreements";
}