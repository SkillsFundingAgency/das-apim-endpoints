using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;

public class GetProviderAccountLegalEntitiesRequest : IGetApiRequest
{
    private readonly int? _ukprn;
    private readonly string _operations;
    private readonly string _accountHashedId;

    public GetProviderAccountLegalEntitiesRequest(int? ukprn, List<Operation> operations)
    {
        _ukprn = ukprn;
        _operations = string.Join('&', operations.Select(o => $"operations={(int)o}"));
    }

    public GetProviderAccountLegalEntitiesRequest(string accountHashedId, List<Operation> operations)
    {
        _accountHashedId = accountHashedId;
        _operations = string.Join('&', operations.Select(o => $"operations={(int)o}"));
    }

    public string GetUrl => $"accountproviderlegalentities?ukprn={_ukprn}&accounthashedid={_accountHashedId}&{_operations}";
}