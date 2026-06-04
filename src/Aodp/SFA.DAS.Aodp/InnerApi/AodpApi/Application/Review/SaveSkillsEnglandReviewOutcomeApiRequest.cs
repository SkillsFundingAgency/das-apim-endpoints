using SFA.DAS.Apim.Shared.Interfaces;

public class SaveSkillsEnglandReviewOutcomeApiRequest(Guid applicationReviewId) : IPutApiRequest
{
    public string PutUrl => $"/api/application-reviews/{applicationReviewId}/skills-england-outcome";

    public object Data { get; set; }
}
