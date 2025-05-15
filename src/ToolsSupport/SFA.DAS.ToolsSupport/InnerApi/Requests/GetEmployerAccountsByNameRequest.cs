using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetEmployerAccountsByNameRequest : IGetApiRequest
{
    private readonly string _employerName;
    public GetEmployerAccountsByNameRequest(string employerName)
    {
        _employerName = employerName;
    }
    public string GetUrl => $"api/accounts/search?employerName={System.Net.WebUtility.UrlEncode(_employerName)}";
} 