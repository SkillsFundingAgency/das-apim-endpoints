namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes
{
    public class GetRoutingInformationForQuestionQueryResponse
    {
        public Guid QuestionId { get; set; }

        public string QuestionTitle { get; set; }
        public string SectionTitle { get; set; }
        public string PageTitle { get; set; }

        public List<RouteInformation> Routes { get; set; } = new();
        public List<RadioOptionItem> RadioOptions { get; set; } = new();
        public List<Page> NextPages { get; set; } = new();
        public List<Page> NextSections { get; set; } = new();
        public bool Editable { get; set; }

        public class RadioOptionItem
        {
            public Guid Id { get; set; }
            public string Value { get; set; }
            public int Order { get; set; }
        }

        public class Section
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int Order { get; set; }

        }

        public class Page
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int Order { get; set; }
        }

        public class RouteInformation
        {
            public Guid Id { get; set; }
            public Guid SourceQuestionId { get; set; }
            public Guid? NextPageId { get; set; }
            public Guid? NextSectionId { get; set; }
            public Guid OptionId { get; set; }
            public bool EndSection { get; set; }
            public bool EndForm { get; set; }

        }

    }
}