using SFA.DAS.SharedOuterApi.Configuration;using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationFormByIdApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }

    public string GetUrl => $"/api/applications/forms/{FormVersionId}";
}
