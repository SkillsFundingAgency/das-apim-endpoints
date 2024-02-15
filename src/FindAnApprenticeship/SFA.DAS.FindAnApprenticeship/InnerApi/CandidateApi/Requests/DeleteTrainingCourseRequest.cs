using SFA.DAS.SharedOuterApi.Interfaces;
using System;


namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class DeleteTrainingCourseRequest(Guid applicationId, Guid candidateId, Guid TrainingCourseId) : IDeleteApiRequest
    {
        public string DeleteUrl => $"candidates/{candidateId}/applications/{applicationId}/trainingcourses/{TrainingCourseId}";
    }
}
