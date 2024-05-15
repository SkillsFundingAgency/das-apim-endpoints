using System;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostAdditionalQuestionApiRequest
{
    public Guid CandidateId { get; set; }
    public string Answer { get; set; }
    public Guid Id { get; set; }
    public int UpdatedAdditionalQuestion { get; set; }
    public SectionStatus AdditionalQuestionSectionStatus { get; set; }
}
