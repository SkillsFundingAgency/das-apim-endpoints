using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;

public class UpdatePageApiRequest : IPutApiRequest
{
    private readonly Guid _pageId;

    public UpdatePageApiRequest(Guid pageId, Page data)
    {
        _pageId = pageId;
        Data = data;
    }

    public string PutUrl => $"/api/pages/{_pageId}";

    public object Data { get; set; }

    public class Page
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Title { get; set; }
        public Guid Key { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int? NextPageId { get; set; }
    }
}