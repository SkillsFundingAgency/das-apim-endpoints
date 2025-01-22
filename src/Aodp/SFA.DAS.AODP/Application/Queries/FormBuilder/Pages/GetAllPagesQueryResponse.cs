using SFA.DAS.Aodp.Application;

namespace SFA.DAS.AODP.Application.FormBuilder.Pages.Queries;

public class GetAllPagesQueryResponse : BaseResponse
{
    public List<Page> Data { get; set; }

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