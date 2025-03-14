using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprovedApprenticeshipsByUlnRequest(string uln) : IGetApiRequest
{
    public readonly string Uln = uln;
    public string GetUrl => $"api/apprenticeships/uln/{Uln}/approved-apprenticeships";
}
