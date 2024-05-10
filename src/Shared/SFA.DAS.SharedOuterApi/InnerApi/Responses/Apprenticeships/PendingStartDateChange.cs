using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;

public class PendingStartDateChange
{
    public DateTime OriginalActualStartDate { get; set; }
    public DateTime PendingActualStartDate { get; set; }
    public string? Reason { get; set; }
    public long? Ukprn { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string? Initiator { get; set; }
    public DateTime? ProviderApprovedDate { get; set; }
    public DateTime? EmployerApprovedDate { get; set; }
}