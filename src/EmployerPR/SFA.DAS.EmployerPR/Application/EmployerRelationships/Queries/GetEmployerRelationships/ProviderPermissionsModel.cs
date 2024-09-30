using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;

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
