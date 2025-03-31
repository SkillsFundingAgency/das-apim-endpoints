using SFA.DAS.SharedOuterApi.Interfaces;

public class GetQualificationVersionApiRequest : IGetApiRequest
{
    private readonly string _qualificationReference;
    public int? _version { get; set; }

    public GetQualificationVersionApiRequest(string qualificationReference,int? version)
    {
        _qualificationReference = qualificationReference;
        _version = version;
    }

    public string GetUrl => $"api/qualifications/{_qualificationReference}/qualificationversions/{_version}";

}