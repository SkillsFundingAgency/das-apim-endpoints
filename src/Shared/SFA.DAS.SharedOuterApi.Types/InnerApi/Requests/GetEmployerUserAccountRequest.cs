using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetEmployerUserAccountRequest(string id) : IGetApiRequest
{
    public string GetUrl => $"api/users/{HttpUtility.UrlEncode(id)}";
}