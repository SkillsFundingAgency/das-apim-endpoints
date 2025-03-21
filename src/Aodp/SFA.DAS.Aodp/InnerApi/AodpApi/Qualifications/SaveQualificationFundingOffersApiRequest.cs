using SFA.DAS.SharedOuterApi.Interfaces;

public class SaveQualificationFundingOffersApiRequest : IPutApiRequest
{
    public Guid QualificationVersionId { get; set; }
    public string PutUrl => $"/api/qualifications/{QualificationVersionId}/save-qualification-funding-offers";
    public object Data { get; set; }
}
