using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PostTrainingCourseApiRequest : IPostApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;

    public PostTrainingCourseApiRequest(Guid applicationId, Guid candidateId, PostTrainingCourseApiRequestData data)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        Data = data;
    }

    public string PostUrl => $"candidates/{_candidateId}/applications/{_applicationId}/training-course";
    public object Data { get; set; }

    public class PostTrainingCourseApiRequestData
    {
        public string CourseName { get; set; }
        public string TrainingProviderName { get; set; }
        public int YearAchieved { get; set; }
    }
}