using SFA.DAS.SharedOuterApi.Interfaces;

public class CreateQualificationDiscussionHistoryApiRequest : IPutApiRequest
{
    public Guid QualificationVersionId { get; set; }

    public string PutUrl => $"/api/qualifications/{QualificationVersionId}/qualification-funding-offers-summary";
    public object Data { get; set; }
}
