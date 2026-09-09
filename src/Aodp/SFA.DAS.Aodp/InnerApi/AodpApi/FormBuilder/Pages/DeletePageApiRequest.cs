using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Pages;

public class DeletePageApiRequest : IDeleteApiRequest
{
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }



    public string DeleteUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageId}";
}