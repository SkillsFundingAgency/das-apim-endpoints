using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
public class GetAdditionalQuestionQueryResult
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
}
