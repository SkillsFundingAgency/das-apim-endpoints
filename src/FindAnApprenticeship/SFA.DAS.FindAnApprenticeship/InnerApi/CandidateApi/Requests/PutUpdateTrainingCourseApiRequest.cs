using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutUpdateTrainingCourseApiRequest : IPutApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    private readonly Guid _id;

    public PutUpdateTrainingCourseApiRequest(Guid applicationId, Guid candidateId, Guid id, PutUpdateTrainingCourseApiRequestData data)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        _id = id;
        Data = data;
    }

    public string PutUrl => $"candidates/{_candidateId}/applications/{_applicationId}/trainingcourses/{_id}";
    public object Data { get; set; }

    public class PutUpdateTrainingCourseApiRequestData
    {
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }
    }
}
