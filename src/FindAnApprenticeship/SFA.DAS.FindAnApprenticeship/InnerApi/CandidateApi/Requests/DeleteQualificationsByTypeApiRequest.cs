using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class DeleteQualificationsByTypeApiRequest(Guid applicationId, Guid candidateId, Guid qualificationReferenceId) : IDeleteApiRequest
    {
        public string DeleteUrl => $"api/candidates/{candidateId}/applications/{applicationId}/qualifications?qualificationReferenceId={qualificationReferenceId}";
    }
}
