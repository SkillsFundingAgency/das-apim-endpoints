using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetAdditionalQuestionApiResponse
{
    public string QuestionText { get; set; }
    public string Answer { get; set; }
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
}
