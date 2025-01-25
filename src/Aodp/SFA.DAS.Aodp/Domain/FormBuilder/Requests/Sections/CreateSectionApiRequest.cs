using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;

public class CreateSectionApiRequest : IPostApiRequest
{
    public Guid FormVersionId { get; set; }

    public string PostUrl => $"/api/forms/{FormVersionId}/sections";

    public object Data { get; set; }

    public class Section
    {

        public string Title { get; set; }
        public string Description { get; set; }
    }
}