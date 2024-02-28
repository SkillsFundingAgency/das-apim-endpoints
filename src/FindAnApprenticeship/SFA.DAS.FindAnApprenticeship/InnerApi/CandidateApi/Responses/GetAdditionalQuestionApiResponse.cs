using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetAdditionalQuestionApiResponse
{
    public Guid Id { get; set; }
    public string QuestionId { get; set; }
    public string Answer { get; set; }
    public Guid ApplicationId { get; set; }
}
