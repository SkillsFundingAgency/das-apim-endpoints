using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
public class GetOrganisationQueryResponse
{
    public Guid OrganisationId { get; set; }
    public int Ukprn { get; set; }
    public string LegalName { get; set; } = string.Empty;
    public string TradingName { get; set; } = string.Empty;
    public string? CompanyNumber { get; set; }
    public string? CharityNumber { get; set; }
    public DateTime? ApplicationDeterminedDate { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public OrganisationStatusEnum Status { get; set; }
    public DateTime? RemovedDate { get; set; }
    public int? RemovedReasonId { get; set; }
    public string? RemovedReason { get; set; }
    public ProviderTypeEnum ProviderType { get; set; }
    public int? OrganisationTypeId { get; set; }
    public string? OrganisationType { get; set; }
    public IEnumerable<AllowedCourseType> AllowedCourseTypes { get; set; } = Enumerable.Empty<AllowedCourseType>();

    public static implicit operator GetOrganisationQueryResponse(GetOrganisationResponse source) => new()
    {
        OrganisationId = source.OrganisationId,
        Ukprn = source.Ukprn,
        LegalName = source.LegalName,
        TradingName = source.TradingName,
        CompanyNumber = source.CompanyNumber,
        CharityNumber = source.CharityNumber,
        ApplicationDeterminedDate = source.ApplicationDeterminedDate,
        LastUpdatedDate = source.LastUpdatedDate,
        Status = source.Status,
        RemovedDate = source.RemovedDate,
        RemovedReasonId = source.RemovedReasonId,
        RemovedReason = source.RemovedReason,
        ProviderType = source.ProviderType,
        OrganisationTypeId = source.OrganisationTypeId,
        OrganisationType = source.OrganisationType,
        AllowedCourseTypes = source.AllowedCourseTypes.Select(c => new AllowedCourseType(c.CourseTypeId, c.CourseTypeName, c.LearningType))
    };
}
