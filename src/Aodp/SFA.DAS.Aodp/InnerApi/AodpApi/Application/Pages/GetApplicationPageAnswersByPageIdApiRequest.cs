using SFA.DAS.Apim.Shared.Interfaces;

public class GetApplicationPageAnswersByPageIdApiRequest : IGetApiRequest
{
    public Guid PageId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }

    public string GetUrl => $"/api/applications/{ApplicationId}/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageId}/Answers";

}