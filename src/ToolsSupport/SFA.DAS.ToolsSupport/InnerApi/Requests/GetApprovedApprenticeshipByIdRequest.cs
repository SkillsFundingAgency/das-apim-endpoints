using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprovedApprenticeshipByIdRequest : IGetApiRequest
{
    public readonly long Id;
    public string GetUrl => $"api/apprenticeships/{Id}/approved-apprenticeship";

    public GetApprovedApprenticeshipByIdRequest(long id)
    {
        Id = id;
    }
}
