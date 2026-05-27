using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetUserAccountsRequest(string userRef) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/user/{userRef}/accounts";

}