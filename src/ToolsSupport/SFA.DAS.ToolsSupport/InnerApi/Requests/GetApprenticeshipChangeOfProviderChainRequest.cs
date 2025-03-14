using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprenticeshipChangeOfProviderChainRequest(long id) : IGetApiRequest
{
    public readonly long Id = id;
    public string GetUrl => $"api/apprenticeships/{Id}/change-of-provider-chain";
}
