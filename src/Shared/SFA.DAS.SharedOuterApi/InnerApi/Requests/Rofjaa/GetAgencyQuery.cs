using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Rofjaa;

public class GetAgencyQuery(long legalEntityId) : IGetApiRequest
{
    public long LegalEntityId { get; } = legalEntityId;
    
    public string GetUrl => $"agencies/{LegalEntityId}";
}
