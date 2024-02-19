using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetDeleteVolunteeringOrWorkExperienceApiRequest : IGetApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    private readonly Guid _id;

    public GetDeleteVolunteeringOrWorkExperienceApiRequest(Guid applicationId, Guid candidateId, Guid id)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        _id = id;

    }
    public string GetUrl =>
        $"candidates/{_candidateId}/applications/{_applicationId}/volunteeringorworkexperience/{_id}";
}
