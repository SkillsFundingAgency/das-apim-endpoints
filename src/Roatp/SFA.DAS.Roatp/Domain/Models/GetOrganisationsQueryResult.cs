using System.Collections.Generic;

namespace SFA.DAS.Roatp.Domain.Models;

public class GetOrganisationsQueryResult
{
    public List<OrganisationModel> Organisations { get; set; } = [];
}
