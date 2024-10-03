using SFA.DAS.EmployerPR.Common;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;

public class GetRequestQueryResult
{
    public Guid RequestId { get; set; }
    public RequestType RequestType { get; set; }
    public long Ukprn { get; set; }
    public required string ProviderName { get; set; }
    public required string RequestedBy { get; set; }
    public DateTime RequestedDate { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string? EmployerOrganisationName { get; set; }
    public string? EmployerContactFirstName { get; set; }
    public string? EmployerContactLastName { get; set; }
    public string? EmployerContactEmail { get; set; }
    public string? EmployerPAYE { get; set; }
    public string? EmployerAORN { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Operation[] Operations { get; set; } = [];
}
