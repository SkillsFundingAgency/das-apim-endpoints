namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Sections;

public class GetAllSectionsQueryResponse 
{
    public List<Section> Data { get; set; }

    public class Section
    {
        public Guid Id { get; set; }
        public Guid FormVersionId { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
