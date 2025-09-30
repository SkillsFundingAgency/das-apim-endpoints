using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class SearchOrganisationResponse
{
    public IEnumerable<Organisation> SearchResults { get; set; }

    public int TotalCount { get; set; }
}