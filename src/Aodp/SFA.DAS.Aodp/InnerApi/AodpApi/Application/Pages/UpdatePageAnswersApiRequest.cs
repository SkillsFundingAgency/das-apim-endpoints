using SFA.DAS.SharedOuterApi.Configuration;using SFA.DAS.SharedOuterApi.Interfaces;

public class UpdatePageAnswersApiRequest(Guid applicationId, Guid pageId, Guid formVersionId, Guid sectionId) : IPutApiRequest
{
    public Guid PageId { get; set; } = pageId;
    public Guid FormVersionId { get; set; } = formVersionId;
    public Guid SectionId { get; set; } = sectionId;
    public Guid ApplicationId { get; set; } = applicationId;

    public string PutUrl => $"/api/applications/{ApplicationId}/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageId}";

    public object Data { get; set; }

}
