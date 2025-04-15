using SFA.DAS.SharedOuterApi.Interfaces;

public class GetFeedbackForQualificationFundingByIdApiRequest : IGetApiRequest
{
    public Guid QualificationVersionId { get; set; }

    public GetFeedbackForQualificationFundingByIdApiRequest(Guid qualificationVersionId)
    {
        QualificationVersionId = qualificationVersionId;
    }

    public string GetUrl => $"/api/qualifications/{QualificationVersionId}/feedback";

}