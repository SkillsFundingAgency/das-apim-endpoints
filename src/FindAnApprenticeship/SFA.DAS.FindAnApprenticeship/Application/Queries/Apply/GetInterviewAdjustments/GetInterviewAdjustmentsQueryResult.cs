using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;
public class GetInterviewAdjustmentsQueryResult
{
    public Guid ApplicationId { get; set; }
    public string InterviewAdjustmentsDescription { get; set; }
    public bool? IsSectionCompleted { get; set; }
}
