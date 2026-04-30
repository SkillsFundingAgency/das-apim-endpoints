using SFA.DAS.Apim.Shared.Interfaces;

public class GetQualificationVersionsForQualificationByReferenceApiRequest : IGetApiRequest
{
    public string QualificationReference { get; set; }

    public GetQualificationVersionsForQualificationByReferenceApiRequest(string qualificationReference)
    {
        QualificationReference = qualificationReference;
    }

    public string GetUrl => $"/api/qualifications/{QualificationReference}/QualificationVersions";

}