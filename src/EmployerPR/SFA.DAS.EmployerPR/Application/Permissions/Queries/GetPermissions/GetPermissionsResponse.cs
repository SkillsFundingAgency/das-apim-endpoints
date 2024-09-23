using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.EmployerPR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsResponse
{
    public List<Operation> Operations { get; set; } = [];
    public string ProviderName { get; set; } = null!;
    public string AccountLegalEntityName { get; set; } = null!;
}