namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetAwardingOrganisationsForRolloverQueryBuilderQueryResponse
{
    public IEnumerable<RolloverQueryBuilderAwardingOrganisation> AwardingOrganisations { get; set; } = [];
}

public class RolloverQueryBuilderAwardingOrganisation
{
    public Guid Id { get; set; }
    public int? Ukprn { get; set; }
    public string? RecognitionNumber { get; set; }
    public string? NameLegal { get; set; }
    public string? NameOfqual { get; set; }
    public string? NameGovUk { get; set; }
    public string? Name_Dsi { get; set; }
    public string? Acronym { get; set; }
}
