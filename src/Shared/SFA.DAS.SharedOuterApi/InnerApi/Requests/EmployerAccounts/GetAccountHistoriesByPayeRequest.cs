using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;

public record GetAccountHistoriesByPayeRequest(string PayeRef) : IGetApiRequest
{
    public string GetUrl => $"api/accounthistories?payeref={PayeRef}";
}
