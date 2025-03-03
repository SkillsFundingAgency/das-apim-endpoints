using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;

public class CreateSectionApiRequest : IPostApiRequest
{
    public Guid FormVersionId { get; set; }

    public string PostUrl => $"/api/forms/{FormVersionId}/sections";

    public object Data { get; set; }
}