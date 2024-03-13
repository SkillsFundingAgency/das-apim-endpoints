using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class PutUpsertAboutYouItemApiResponse
{
    public Guid Id { get; set; }
    public string Strengths { get; set; }
    public string Improvements { get; set; }
    public string HobbiesAndInterests { get; set; }
    public string Support { get; set; }
    public Guid ApplicationId { get; set; }
}
