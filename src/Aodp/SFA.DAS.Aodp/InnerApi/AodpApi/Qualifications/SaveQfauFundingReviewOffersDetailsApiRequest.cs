using SFA.DAS.SharedOuterApi.Interfaces;

public class SaveQualificationFundingOffersDetailsApiRequest : IPutApiRequest
{
    public Guid QualificationVersionId { get; set; }

    public string PutUrl => $"/api/qualifications/{QualificationVersionId}/save-qualification-funding-offers-details";
    public object Data { get; set; }
}
