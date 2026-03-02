using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class OrganisationResponse
{
    public Guid OrganisationId { get; set; }
    public int Ukprn { get; set; }
    public string LegalName { get; set; }
    public string TradingName { get; set; }
    public string CompanyNumber { get; set; }
    public string CharityNumber { get; set; }
    public Common.ProviderType ProviderType { get; set; }
    public int OrganisationTypeId { get; set; }
    public string OrganisationType { get; set; }
    public Common.OrganisationStatus Status { get; set; }
    public DateTime StatusDate { get; set; }
    public DateTime? ApplicationDeterminedDate { get; set; }
    public int? RemovedReasonId { get; set; }
    public string RemovedReason { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public IEnumerable<AllowedCourseType> AllowedCourseTypes { get; set; } = [];
}
