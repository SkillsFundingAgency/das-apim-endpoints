using System.Net;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetUsersByEmailRequest(string email) : IGetApiRequest
{
    public string GetUrl => $"api/users/query?email={WebUtility.UrlEncode(email)}";
}