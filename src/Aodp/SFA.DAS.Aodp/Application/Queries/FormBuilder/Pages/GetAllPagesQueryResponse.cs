namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;

public class GetAllPagesQueryResponse
{
    public List<Page> Data { get; set; }

    public class Page
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid Key { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
    }

}