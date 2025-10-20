using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
public class GetOrganisationsQueryResult
{
    public IEnumerable<OrganisationSummary> Organisations { get; set; } = Enumerable.Empty<OrganisationSummary>();
}

public record OrganisationSummary(int Ukprn, string LegalName)
{
    public static implicit operator OrganisationSummary(GetOrganisationDetails source) => new(source.Ukprn, source.LegalName);
}