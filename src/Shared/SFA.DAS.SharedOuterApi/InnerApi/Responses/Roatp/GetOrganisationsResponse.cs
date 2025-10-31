using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class GetOrganisationsResponse
{
    public List<OrganisationResponse> Organisations { get; set; } = new();
}