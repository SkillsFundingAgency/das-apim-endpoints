using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;

namespace SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;

public class ProviderPermissionsModel
{
    public required long Ukprn { get; set; }

    public required string ProviderName { get; set; }

    public Operation[] Operations { get; set; } = [];

    public static implicit operator ProviderPermissionsModel(ProviderPermissionsResponseModel source) => new()
    {
        Ukprn = source.Ukprn,
        ProviderName = source.ProviderName,
        Operations = source.Operations,
    };
}
