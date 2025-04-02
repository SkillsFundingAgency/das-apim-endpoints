using SFA.DAS.SharedOuterApi.Interfaces;

public class SaveQualificationFundingOffersOutcomeApiRequest : IPutApiRequest
{
    public Guid QualificationVersionId { get; set; }
    public string PutUrl => $"/api/qualifications/{QualificationVersionId}/save-qualification-funding-offers-outcome";
    public object Data { get; set; }
}
