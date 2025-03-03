namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes
{
    public class GetAvailableSectionsAndPagesForRoutingQueryResponse
    {
        public List<Section> Sections { get; set; } = new();

        public class Section
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int Order { get; set; }

            public List<Page> Pages { get; set; } = new();
        }

        public class Page
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int Order { get; set; }
        }
    }
}