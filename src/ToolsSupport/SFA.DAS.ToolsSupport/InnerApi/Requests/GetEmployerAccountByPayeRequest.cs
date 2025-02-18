using System.Net;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetEmployerAccountByPayeRequest(string payeRef) : IGetApiRequest
{
    public string GetUrl => $"api/accounthistories?payeRef={WebUtility.UrlEncode(payeRef)}";
}

