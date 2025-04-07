namespace SFA.DAS.Aodp.Application.Queries.Application.Section;

public class GetApplicationSectionStatusByApplicationIdQueryResponse
{
    public List<Page> Pages { get; set; } = new();

    public class Page
    {
        public Guid PageId { get; set; }
        public string Status { get; set; }
    }

}