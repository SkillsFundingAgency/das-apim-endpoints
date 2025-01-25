using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;

public class CreatePageApiRequest : IPostApiRequest
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string PostUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/Pages";

    public object Data { get; set; }

    public class Page
    {

        public string Title { get; set; }
        public string Description { get; set; }
    }
}