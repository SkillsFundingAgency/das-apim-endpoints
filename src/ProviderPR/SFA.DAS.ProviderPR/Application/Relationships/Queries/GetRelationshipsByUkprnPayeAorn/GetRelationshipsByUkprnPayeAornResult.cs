using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipsByUkprnPayeAorn;
public class GetRelationshipsByUkprnPayeAornResult
{
    public bool HasActiveRequest { get; set; }
    public bool? HasInvalidPaye { get; set; }

    public OrganisationDetails? Organisation { get; set; }

    public AccountDetails? Account { get; set; }

    public bool? HasOneLegalEntity { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string? AccountLegalEntityName { get; set; }
    public bool? HasRelationship { get; set; }
    public List<Operation> Operations { get; set; } = new List<Operation>();
}

public class AccountDetails
{
    public long AccountId { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime? RemovedDate { get; set; }
}

public class OrganisationDetails
{
    public string? Name { get; set; }
    public AddressDetails Address { get; set; } = new();
}

public class AddressDetails
{
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Line3 { get; set; }
    public string? Line4 { get; set; }
    public string? Line5 { get; set; }
    public string? Postcode { get; set; }
}