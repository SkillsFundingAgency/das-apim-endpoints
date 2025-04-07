namespace SFA.DAS.Aodp.Application.Queries.Application.Section;

public class GetApplicationSectionByIdQueryResponse
{
    public string SectionTitle { get; set; }

    public List<Page> Pages { get; set; } = new();

    public class Page
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}