namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

public class GetOrganisationTypesResponse
{
    public IEnumerable<OrganisationTypeSummary> OrganisationTypes { get; set; } = Enumerable.Empty<OrganisationTypeSummary>();
}