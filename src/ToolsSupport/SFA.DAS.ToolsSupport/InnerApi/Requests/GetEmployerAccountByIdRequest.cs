using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetEmployerAccountByIdRequest(long id) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{id}";
}

