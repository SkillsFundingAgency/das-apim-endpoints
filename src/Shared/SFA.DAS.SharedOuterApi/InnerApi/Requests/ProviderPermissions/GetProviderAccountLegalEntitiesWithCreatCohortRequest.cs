using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Interfaces;
using Operation = SFA.DAS.SharedOuterApi.Models.ProviderRelationships.Operation;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;

public class GetProviderAccountLegalEntitiesWithCreatCohortRequest : IGetApiRequest
{
    private readonly int? _ukprn;
    private readonly string _operations;

    public GetProviderAccountLegalEntitiesWithCreatCohortRequest(int? ukprn, List<Operation> operations)
    {
        _ukprn = ukprn;
        _operations = string.Join('&', operations.Select(o => $"operations={(int)o}"));
    }

    public string GetUrl => $"accountproviderlegalentities?ukprn={_ukprn}&{_operations}";
}