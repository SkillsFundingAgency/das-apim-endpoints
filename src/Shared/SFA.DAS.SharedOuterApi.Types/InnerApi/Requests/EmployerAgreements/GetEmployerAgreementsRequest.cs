using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAgreements;

public record GetEmployerAgreementsRequest(long AccountId) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/accounts/{AccountId}/agreements";
}