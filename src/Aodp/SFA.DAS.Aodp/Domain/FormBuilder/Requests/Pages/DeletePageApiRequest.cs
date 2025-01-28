using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Pages;

public class DeletePageApiRequest : IDeleteApiRequest
{
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }



    public string DeleteUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageId}";
}