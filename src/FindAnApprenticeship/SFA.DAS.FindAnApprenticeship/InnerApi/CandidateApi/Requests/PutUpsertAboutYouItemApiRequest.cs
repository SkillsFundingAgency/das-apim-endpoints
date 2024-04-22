#nullable enable
using System;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutUpsertAboutYouItemApiRequest : IPutApiRequest
{
    private readonly Guid _applicationId;
    private readonly Guid _candidateId;
    private readonly Guid _id;

    public PutUpsertAboutYouItemApiRequest(Guid applicationId, Guid candidateId, Guid id, PutUpdateAboutYouItemApiRequestData data)
    {
        _applicationId = applicationId;
        _candidateId = candidateId;
        _id = id;
        Data = data;
    }

    public string PutUrl => $"candidates/{_candidateId}/applications/{_applicationId}/about-you/{_id}";
    public object Data { get; set; }

    public class PutUpdateAboutYouItemApiRequestData
    {
        public string? SkillsAndStrengths { get; set; }
        public string? HobbiesAndInterests { get; set; }
        public string? Improvements { get; set; }
        public string? Support { get; set; }
        public GenderIdentity? Sex { get; set; }
        public EthnicGroup? EthnicGroup { get; set; }
        public EthnicSubGroup? EthnicSubGroup { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; set; }
    }
}
