namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Sections;

public class GetSectionByIdQueryResponse
{

    public Guid Id { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid Key { get; set; }
    public int Order { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Page> Pages { get; set; }
    public bool Editable { get; set; }
    public bool HasAssociatedRoutes { get; set; }

    public class Page
    {
        public Guid Id { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
    }
}