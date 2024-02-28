using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
public class GetAdditionalQuestionQueryResult
{
    public Guid Id { get; set; }
    public string QuestionId { get; set; }
    public string Answer { get; set; }
    public Guid ApplicationId { get; set; }
}
