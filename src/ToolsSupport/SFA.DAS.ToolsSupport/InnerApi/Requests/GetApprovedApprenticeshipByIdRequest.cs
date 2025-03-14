using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprovedApprenticeshipByIdRequest(long id) : IGetApiRequest
{
    public readonly long Id = id;
    public string GetUrl => $"api/apprenticeships/{Id}/approved-apprenticeship";
}
