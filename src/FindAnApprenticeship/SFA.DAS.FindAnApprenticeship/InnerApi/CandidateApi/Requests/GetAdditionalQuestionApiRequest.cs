using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetAdditionalQuestionApiRequest : IGetApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    private readonly Guid _id;

    public GetAdditionalQuestionApiRequest(Guid applicationId, Guid candidateId, Guid id)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        _id = id;
    }

    public string GetUrl =>
           $"candidates/{_candidateId}/applications/{_applicationId}/additional-question/{_id}";
}
