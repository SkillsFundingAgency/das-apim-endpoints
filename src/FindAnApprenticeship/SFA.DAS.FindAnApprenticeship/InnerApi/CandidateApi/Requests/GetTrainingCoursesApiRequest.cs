using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetTrainingCoursesApiRequest : IGetApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;

    public GetTrainingCoursesApiRequest(Guid applicationId, Guid candidateId)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
    }

    public string GetUrl =>
        $"candidates/{_candidateId}/applications/{_applicationId}/trainingcourses";
}
