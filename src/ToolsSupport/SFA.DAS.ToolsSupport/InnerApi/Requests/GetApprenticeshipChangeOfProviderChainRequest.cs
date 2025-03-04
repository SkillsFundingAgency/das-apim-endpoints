using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprenticeshipChangeOfProviderChainRequest : IGetApiRequest
{
    public readonly long Id;
    public string GetUrl => $"api/apprenticeships/{Id}/change-of-provider-chain";

    public GetApprenticeshipChangeOfProviderChainRequest(long id)
    {
        Id = id;
    }
}
