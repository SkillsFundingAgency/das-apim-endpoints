using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetAdditionalQuestionApiRequest : IGetApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    private readonly Guid _questionId;

    public GetAdditionalQuestionApiRequest(Guid applicationId, Guid candidateId, Guid questionId)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        _questionId = questionId;
    }

    public string GetUrl =>
           $"candidates/{_candidateId}/applications/{_applicationId}/additional-question/{_questionId}";
}
