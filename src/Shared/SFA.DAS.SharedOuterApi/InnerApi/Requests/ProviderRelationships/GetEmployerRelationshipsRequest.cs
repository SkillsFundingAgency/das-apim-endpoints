using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;

#nullable enable

public class GetEmployerRelationshipsRequest : IGetApiRequest
{
    public string GetUrl => $"relationships/employeraccount/{AccountHashedId}?Ukprn={Ukprn}&AccountlegalentityPublicHashedId={AccountlegalentityPublicHashedId}";
    public string AccountHashedId { get; set; }

    public long? Ukprn { get; set; }

    public string? AccountlegalentityPublicHashedId { get; set; }

    public GetEmployerRelationshipsRequest(string accountHashedId, long? ukprn = null, string? accountlegalentityPublicHashedId = null)
    {
        AccountHashedId = accountHashedId;
        Ukprn = ukprn;
        AccountlegalentityPublicHashedId = accountlegalentityPublicHashedId;
    }
}
