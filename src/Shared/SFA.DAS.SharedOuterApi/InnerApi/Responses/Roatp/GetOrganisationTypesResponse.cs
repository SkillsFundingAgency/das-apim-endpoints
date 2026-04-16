using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class GetOrganisationTypesResponse
{
    public IEnumerable<OrganisationTypeSummary> OrganisationTypes { get; set; } = Enumerable.Empty<OrganisationTypeSummary>();
}