using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAgreements;

public record GetEmployerAgreementsRequest(long AccountId) : IGetAllApiRequest 
{
    public string GetAllUrl => $"api/accounts/{AccountId}/agreements";
}