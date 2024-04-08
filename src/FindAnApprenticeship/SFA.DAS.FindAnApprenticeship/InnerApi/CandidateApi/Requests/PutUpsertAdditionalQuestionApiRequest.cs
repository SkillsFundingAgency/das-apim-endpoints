using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutUpsertAdditionalQuestionApiRequest : IPutApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    private readonly Guid _id;

    public PutUpsertAdditionalQuestionApiRequest(Guid applicationId, Guid candidateId, Guid id, PutUpsertAdditionalQuestionApiRequestData data)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        _id = id;
        Data = data;
    }

    public string PutUrl => $"candidates/{_candidateId}/applications/{_applicationId}/additional-question/{_id}";
    public object Data { get; set; }

    public class PutUpsertAdditionalQuestionApiRequestData
    {
        public string Answer { get; set; }
    }
}
