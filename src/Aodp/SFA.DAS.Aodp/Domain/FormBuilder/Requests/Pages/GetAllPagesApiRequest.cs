using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;

public class GetAllPagesApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string GetUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/Pages";
}