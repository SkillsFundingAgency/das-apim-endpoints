using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetQualificationsApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
{
    public string GetUrl =>
        $"candidates/{candidateId}/applications/{applicationId}/qualifications";
}