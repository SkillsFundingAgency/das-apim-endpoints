using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Rofjaa;

public class GetAgencyQuery(long legalEntityId) : IGetApiRequest
{
    public long LegalEntityId { get; } = legalEntityId;
    public string GetUrl => $"agencies/{LegalEntityId}";
}
