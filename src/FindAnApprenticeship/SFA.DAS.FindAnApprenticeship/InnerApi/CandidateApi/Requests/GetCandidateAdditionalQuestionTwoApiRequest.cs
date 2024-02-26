using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetCandidateAdditionalQuestionTwoApiRequest : IGetApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;

    public GetCandidateAdditionalQuestionTwoApiRequest(Guid applicationId, Guid candidateId)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
    }

    //todo: update GetUrl
    public string GetUrl =>
           $"candidates/{_candidateId}/applications/{_applicationId}/";
}
