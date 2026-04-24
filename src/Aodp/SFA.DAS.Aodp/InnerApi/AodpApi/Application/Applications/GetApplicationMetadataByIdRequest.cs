using SFA.DAS.Apim.Shared.Interfaces;

public class GetApplicationMetadataByIdRequest : IGetApiRequest
{
    public Guid ApplicationId { get; set; }

    public string GetUrl => $"/api/applications/{ApplicationId}/metadata";

}
