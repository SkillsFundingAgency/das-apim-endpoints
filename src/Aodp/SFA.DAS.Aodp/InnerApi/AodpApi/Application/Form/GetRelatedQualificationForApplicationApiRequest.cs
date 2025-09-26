using SFA.DAS.SharedOuterApi.Interfaces;

public class GetRelatedQualificationForApplicationApiRequest : IGetApiRequest
{
    public Guid ApplicationId { get; set; }

    public string GetUrl => $"/api/applications/{ApplicationId}/qualification";
}
