using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;

public class MoveSectionDownApiRequest : IPutApiRequest
{

    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string PutUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/MoveDown";
    public object Data { get; set; }
}