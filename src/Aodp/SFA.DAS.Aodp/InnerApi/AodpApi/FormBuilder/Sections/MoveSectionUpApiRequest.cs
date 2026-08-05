using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;

public class MoveSectionUpApiRequest : IPutApiRequest
{

    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string PutUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/MoveUp";
    public object Data { get; set; }
}