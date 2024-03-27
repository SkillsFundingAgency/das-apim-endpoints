using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetQualificationsApiRequest(Guid applicationId, Guid candidateId, Guid? qualificationTypeId = null) : IGetApiRequest
{
    public string GetUrl => qualificationTypeId.HasValue 
        ? $"api/candidates/{candidateId}/applications/{applicationId}/qualifications?qualificationTypeId={qualificationTypeId.Value}"
        : $"api/candidates/{candidateId}/applications/{applicationId}/qualifications";
}