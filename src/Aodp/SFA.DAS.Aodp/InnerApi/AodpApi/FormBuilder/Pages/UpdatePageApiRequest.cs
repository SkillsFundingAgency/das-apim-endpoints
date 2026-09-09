using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Pages;

public class UpdatePageApiRequest : IPutApiRequest
{
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string PutUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageId}";

    public object Data { get; set; }
}