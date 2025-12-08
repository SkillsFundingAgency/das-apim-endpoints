using SFA.DAS.Approvals.Application.Shared.Enums;

namespace SFA.DAS.Approvals.Api.Models.DraftApprenticeships;

public class DraftApprenticeshipSetReferenceRequest
{
    public long? CohortId { get; set; }
    public long? ApprenticeshipId { get; set; }
    public string Reference { get; set; }
    public Party? Party { get; set; }
}
