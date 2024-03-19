using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetInterviewAdjustmentsApiResponse
{
    public Guid ApplicationId { get; set; }
    public string InterviewAdjustmentsDescription { get; set; }
    public bool? Status { get; set; }

    public static implicit operator GetInterviewAdjustmentsApiResponse(GetInterviewAdjustmentsQueryResult source)
    {
        return new GetInterviewAdjustmentsApiResponse()
        {
            ApplicationId = source.ApplicationId,
            InterviewAdjustmentsDescription = source.InterviewAdjustmentsDescription,
            Status = source.IsSectionCompleted
        };
    }
}
