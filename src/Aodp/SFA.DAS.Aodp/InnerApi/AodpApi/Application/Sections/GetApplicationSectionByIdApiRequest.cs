using SFA.DAS.SharedOuterApi.Configuration;using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationSectionByIdApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string GetUrl => $"/api/applications/forms/{FormVersionId}/sections/{SectionId}";
}
