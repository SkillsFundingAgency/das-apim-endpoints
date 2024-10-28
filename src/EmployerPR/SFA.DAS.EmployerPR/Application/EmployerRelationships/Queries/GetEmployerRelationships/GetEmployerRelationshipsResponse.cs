using SFA.DAS.EmployerPR.Common;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsResponse
{
    public GetEmployerRelationshipsResponse()
    {

    }
    public List<AccountLegalEntityPermissionsResponseModel> AccountLegalEntities { get; set; } = [];
}

public class AccountLegalEntityPermissionsResponseModel
{
    public required long Id { get; set; }

    public required string PublicHashedId { get; set; }

    public required string Name { get; set; }

    public required long AccountId { get; set; }

    public List<ProviderPermissionsResponseModel> Permissions { get; set; } = [];
    public List<ProviderRequestsResponseModel> Requests { get; set; } = [];
}

public class ProviderPermissionsResponseModel
{
    public required long Ukprn { get; set; }

    public required string ProviderName { get; set; }

    public Operation[] Operations { get; set; } = [];
}

public class ProviderRequestsResponseModel
{
    public required long Ukprn { get; set; }
    public required string ProviderName { get; set; }
    public required Guid RequestId { get; set; }
    public Operation[] Operations { get; set; } = [];
    public RequestType RequestType { get; set; }
}

