using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostInterviewAdjustmentsApiRequest
{
    public Guid CandidateId { get; set; }
    public string InterviewAdjustmentsDescription { get; set; }
    public SectionStatus InterviewAdjustmentsSectionStatus { get; set; }
}
