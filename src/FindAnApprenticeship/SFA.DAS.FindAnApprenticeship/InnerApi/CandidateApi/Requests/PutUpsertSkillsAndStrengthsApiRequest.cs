using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutUpsertSkillsAndStrengthsApiRequest : IPutApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    private readonly Guid _id;

    public PutUpsertSkillsAndStrengthsApiRequest(Guid applicationId, Guid candidateId, Guid id, PutUpdateSkillsAndStrengthsApiRequestData data)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        _id = id;
        Data = data;
    }

    public string PutUrl => $"candidates/{_candidateId}/applications/{_applicationId}/about-you/{_id}";
    public object Data { get; set; }

    public class PutUpdateSkillsAndStrengthsApiRequestData
    {
        public string? SkillsAndStrengths { get; set; }
    }
}
