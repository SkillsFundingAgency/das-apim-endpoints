using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public class GetCeasedPayeSchemesRequest(long accountId) : IGetApiRequest
{
    public string GetUrl => $"api/levy-declarations/{accountId}/ceased";
}