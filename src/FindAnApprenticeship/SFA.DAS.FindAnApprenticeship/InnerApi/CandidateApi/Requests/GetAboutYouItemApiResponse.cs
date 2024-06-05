using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetAboutYouItemApiResponse
{
    public AboutYouItem AboutYou { get; set; }
}

public class AboutYouItem
{
    public string SkillsAndStrengths { get; set; }
    public string Support { get; set; }
    public GenderIdentity? Sex { get; set; }
    public EthnicGroup? EthnicGroup { get; set; }
    public EthnicSubGroup? EthnicSubGroup { get; set; }
    public string? IsGenderIdentifySameSexAtBirth { get; set; }
    public string? OtherEthnicSubGroupAnswer { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid Id { get; set; }
}
