using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class PutUpsertAdditionalQuestionApiResponse
{
    public Guid Id { get; init; }
    public Guid CandidateId { get; set; }
    public string Answer { get; init; }
    public Guid ApplicationId { get; init; }
    public string QuestionId { get; init; }
}
