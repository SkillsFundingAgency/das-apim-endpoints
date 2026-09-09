using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Pages;

public class GetAllPagesApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string GetUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/Pages";
}