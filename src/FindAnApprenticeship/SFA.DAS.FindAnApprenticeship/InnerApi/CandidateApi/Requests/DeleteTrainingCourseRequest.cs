using SFA.DAS.SharedOuterApi.Interfaces;
using System;


namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class DeleteTrainingCourseRequest(Guid applicationId, Guid candidateId, Guid trainingCourseId) : IDeleteApiRequest
    {
        public string DeleteUrl => $"candidates/{candidateId}/applications/{applicationId}/trainingcourses/{trainingCourseId}";
    }
}
