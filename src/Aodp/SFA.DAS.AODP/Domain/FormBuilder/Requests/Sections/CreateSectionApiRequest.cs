using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Sections;

public class CreateSectionApiRequest : IPostApiRequest
{
    public Guid FormVersionId { get; set; }

    public string PostUrl => $"/api/forms/{FormVersionId}/sections";

    public object Data { get; set; }
}