using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerPR.Application.Queries.GetPermissions;
public class GetPermissionsResponse
{
    public List<Operation> Operations { get; set; } = [];
}