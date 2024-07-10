using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;

public class GetProviderAccountLegalEntitiesWithCreatCohortRequest : IGetApiRequest
{
    private readonly int? _ukprn;

    public GetProviderAccountLegalEntitiesWithCreatCohortRequest(int? ukprn)
    {
        _ukprn = ukprn;
    }

    public string GetUrl => $"accountproviderlegalentities?ukprn={_ukprn}&operations=0";
}