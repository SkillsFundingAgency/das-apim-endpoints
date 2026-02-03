using System;

namespace SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Responses;

public class EvsCheckResponse
{
    public long EmployerId { get; set; }
    public long ApprenticeshipId { get; set; }
    public string Ukprn { get; set; }
    public string Uln { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? DateOfCheck { get; set; }
    public EvsCheckResult Result { get; set; }
}
