using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprovedApprenticeshipsByUlnRequest : IGetApiRequest
{
    public readonly string Uln;
    public string GetUrl => $"api/apprenticeships/uln/{Uln}/approved-apprenticeships";

    public GetApprovedApprenticeshipsByUlnRequest(string uln)
    {
        Uln = uln;
    }
}
