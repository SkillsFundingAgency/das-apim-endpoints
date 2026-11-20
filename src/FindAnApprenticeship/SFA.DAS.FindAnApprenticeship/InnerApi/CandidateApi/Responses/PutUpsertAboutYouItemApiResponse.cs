using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class PutUpsertAboutYouItemApiResponse
{
    public Guid Id { get; set; }
    public string Strengths { get; set; }
    public string Support { get; set; }
    public GenderIdentity? Sex { get; set; }
    public EthnicGroup? EthnicGroup { get; set; }
    public EthnicSubGroup? EthnicSubGroup { get; set; }
    public string? IsGenderIdentifySameSexAtBirth { get; set; }
    public string? OtherEthnicSubGroupAnswer { get; set; }
    public Guid ApplicationId { get; set; }
}
