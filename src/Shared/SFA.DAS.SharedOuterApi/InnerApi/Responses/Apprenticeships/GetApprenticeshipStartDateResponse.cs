using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;

public class GetApprenticeshipStartDateResponse
{
    public Guid ApprenticeshipKey { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public long UKPRN { get; set; }
    public int CourseCode { get; set; }
}