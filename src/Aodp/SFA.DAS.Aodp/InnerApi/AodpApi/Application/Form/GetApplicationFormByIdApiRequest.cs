using SFA.DAS.Apim.Shared.Interfaces;

public class GetApplicationFormByIdApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }

    public string GetUrl => $"/api/applications/forms/{FormVersionId}";
}
