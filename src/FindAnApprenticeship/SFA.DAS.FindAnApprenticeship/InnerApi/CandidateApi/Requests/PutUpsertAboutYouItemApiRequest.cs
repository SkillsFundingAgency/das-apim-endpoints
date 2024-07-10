#nullable enable
using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutUpsertAboutYouItemApiRequest : IPutApiRequest
{
    private readonly Guid _candidateId;

    public PutUpsertAboutYouItemApiRequest(Guid candidateId, PutUpdateAboutYouItemApiRequestData data)
    {
        _candidateId = candidateId;
        Data = data;
    }

    public string PutUrl => $"api/candidates/{_candidateId}/about-you";
    public object Data { get; set; }

    public class PutUpdateAboutYouItemApiRequestData
    {
        public GenderIdentity? Sex { get; set; }
        public EthnicGroup? EthnicGroup { get; set; }
        public EthnicSubGroup? EthnicSubGroup { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; set; }
    }
}
