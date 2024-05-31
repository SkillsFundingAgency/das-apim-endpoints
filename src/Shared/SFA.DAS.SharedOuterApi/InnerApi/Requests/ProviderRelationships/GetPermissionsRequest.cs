using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;

public class GetPermissionsRequest : IGetApiRequest
{
    public string GetUrl => $"permissions?Ukprn={Ukprn}&accountLegalEntityId={AccountLegalEntityId}";

    public long? Ukprn { get; }

    public int? AccountLegalEntityId { get; set; }

    public GetPermissionsRequest(long? ukprn, int? accountLegalEntityId)
    {
        Ukprn = ukprn;
        AccountLegalEntityId = accountLegalEntityId;
    }
}