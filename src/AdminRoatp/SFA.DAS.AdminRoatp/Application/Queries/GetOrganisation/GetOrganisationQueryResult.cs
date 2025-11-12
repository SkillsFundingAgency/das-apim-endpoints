using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;

public class GetOrganisationQueryResult
{
    public Guid OrganisationId { get; set; }
    public int Ukprn { get; set; }
    public required string LegalName { get; set; }
    public string? TradingName { get; set; }
    public string? CompanyNumber { get; set; }
    public string? CharityNumber { get; set; }
    public ProviderType ProviderType { get; set; }
    public int OrganisationTypeId { get; set; }
    public required string OrganisationType { get; set; }
    public OrganisationStatus Status { get; set; }
    public DateTime? ApplicationDeterminedDate { get; set; }
    public int? RemovedReasonId { get; set; }
    public string? RemovedReason { get; set; }
    public DateTime? RemovedDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public IEnumerable<AllowedCourseType> AllowedCourseTypes { get; set; } = [];

    public static implicit operator GetOrganisationQueryResult(OrganisationResponse source) => new()
    {
        OrganisationId = source.OrganisationId,
        Ukprn = source.Ukprn,
        LegalName = source.LegalName,
        TradingName = source.TradingName,
        CompanyNumber = source.CompanyNumber,
        CharityNumber = source.CharityNumber,
        ProviderType = source.ProviderType,
        OrganisationTypeId = source.OrganisationTypeId,
        OrganisationType = source.OrganisationType,
        Status = source.Status,
        ApplicationDeterminedDate = source.ApplicationDeterminedDate,
        RemovedReasonId = source.RemovedReasonId,
        RemovedReason = source.RemovedReason,
        StartDate = source.StartDate,
        LastUpdatedDate = source.LastUpdatedDate,
        AllowedCourseTypes = source.AllowedCourseTypes
    };
}
