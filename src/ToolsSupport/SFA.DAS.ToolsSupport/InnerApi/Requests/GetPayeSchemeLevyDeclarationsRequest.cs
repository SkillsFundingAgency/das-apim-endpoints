using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

internal class GetPayeSchemeLevyDeclarationsRequest(string payeScheme) : IGetApiRequest
{
    public string GetUrl => $"apprenticeship-levy/epaye/{HttpUtility.UrlEncode(payeScheme)}/declarations";
}