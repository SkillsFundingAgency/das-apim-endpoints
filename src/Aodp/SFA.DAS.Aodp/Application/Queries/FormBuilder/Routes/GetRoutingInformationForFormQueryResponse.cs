namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes
{

    public class GetRoutingInformationForFormQueryResponse
    {
        public List<Section> Sections { get; set; }
        public bool Editable { get; set; }

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
            public Question Quesiton { get; set; } = new();
        }

        public class Question
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int Order { get; set; }
            public List<RouteInformation> Routes { get; set; } = new();
        }

        public class RouteInformation
        {
            public Page? NextPage { get; set; }
            public Section? NextSection { get; set; }
            public bool EndForm { get; set; }
            public bool EndSection { get; set; }
            public RadioOptionItem Option { get; set; }
        }

        public class RadioOptionItem
        {
            public Guid Id { get; set; }
            public string Value { get; set; }
            public int Order { get; set; }
        }
    }
}