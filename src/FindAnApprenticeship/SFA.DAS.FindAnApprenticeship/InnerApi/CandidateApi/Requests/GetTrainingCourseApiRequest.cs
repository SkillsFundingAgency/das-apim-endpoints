using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetTrainingCourseApiRequest : IGetApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    private readonly Guid _trainingCourseId;

    public GetTrainingCourseApiRequest(Guid applicationId, Guid candidateId, Guid trainingCourseId)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        _trainingCourseId = trainingCourseId;
    }

    public string GetUrl =>
        $"candidates/{_candidateId}/applications/{_applicationId}/trainingcourses/{_trainingCourseId}";
}
