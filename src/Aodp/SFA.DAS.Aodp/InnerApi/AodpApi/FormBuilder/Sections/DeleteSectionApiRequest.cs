using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;

public class DeleteSectionApiRequest : IDeleteApiRequest
{

    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string DeleteUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}";
}