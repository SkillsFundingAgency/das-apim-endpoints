using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostAdditionalQuestionApiRequest
{
    public Guid CandidateId { get; set; }
    public string Answer { get; set; }
    public Guid QuestionId { get; set; }
}
