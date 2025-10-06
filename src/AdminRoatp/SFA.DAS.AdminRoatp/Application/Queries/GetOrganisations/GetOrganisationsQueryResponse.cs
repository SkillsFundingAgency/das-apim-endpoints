using InnerResponse = SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
public class GetOrganisationsQueryResponse
{
    public IEnumerable<Organisation> Organisations { get; set; } = Enumerable.Empty<Organisation>();
}

public record Organisation(Guid OrganisationId, long Ukprn, string LegalName)
{
    public static implicit operator Organisation(InnerResponse.Organisation source)
        => new(source.Id, source.UKPRN, source.LegalName);
}