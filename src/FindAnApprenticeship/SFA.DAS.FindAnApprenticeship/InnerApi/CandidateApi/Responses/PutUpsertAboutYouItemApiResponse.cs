using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class PutUpsertAboutYouItemApiResponse
{
    public Guid Id { get; set; }
    public string Strengths { get; set; }
    public string Improvements { get; set; }
    public string HobbiesAndInterests { get; set; }
    public string Support { get; set; }
    public string? Sex { get; set; }
    public string? EthnicGroup { get; set; }
    public string? EthnicSubGroup { get; set; }
    public string? IsGenderIdentifySameSexAtBirth { get; set; }
    public string? OtherEthnicSubGroupAnswer { get; set; }
    public Guid ApplicationId { get; set; }
}
