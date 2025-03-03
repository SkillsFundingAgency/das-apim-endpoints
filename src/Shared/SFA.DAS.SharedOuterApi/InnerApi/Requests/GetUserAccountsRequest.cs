using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetUserAccountsRequest(string userRef) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/user/{userRef}/accounts";
    
}