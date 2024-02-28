using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutUpsertAdditionalQuestionApiRequest : IPutApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;

    public PutUpsertAdditionalQuestionApiRequest(Guid applicationId, Guid candidateId, PutUpsertAdditionalQuestionApiRequestData data)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        Data = data;
    }

    public string PutUrl => $"candidates/{_candidateId}/applications/{_applicationId}/additional-question";
    public object Data { get; set; }

    public class PutUpsertAdditionalQuestionApiRequestData
    {
        public string Answer { get; set; }
        public Guid QuestionId { get; set; }
    }
}
