using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Sections;

public class DeleteSectionApiRequest : IDeleteApiRequest
{

    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string DeleteUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}";
}