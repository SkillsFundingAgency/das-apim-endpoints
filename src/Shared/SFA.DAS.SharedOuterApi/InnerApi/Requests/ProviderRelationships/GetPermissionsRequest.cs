using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;

public class GetPermissionsRequest : IGetApiRequest
{
    public string GetUrl => $"permissions?Ukprn={Ukprn}&PublicHashedId={PublicHashedId}";

    public long? Ukprn { get; }

    public string? PublicHashedId { get; set; }

    public GetPermissionsRequest(long? ukprn, string? publicHashedId)
    {
        Ukprn = ukprn;
        PublicHashedId = publicHashedId;
    }
}