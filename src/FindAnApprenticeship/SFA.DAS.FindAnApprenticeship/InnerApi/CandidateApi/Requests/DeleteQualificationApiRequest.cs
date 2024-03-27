using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class DeleteQualificationApiRequest(Guid candidateId, Guid applicationId, Guid id) : IDeleteApiRequest
{
    public string DeleteUrl => $"api/candidates/{candidateId}/applications/{applicationId}/Qualifications/{id}";
}