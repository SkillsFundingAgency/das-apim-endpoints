public class GetApplicationFormPreviewByIdQueryResponse
{
    public Guid ApplicationId { get; set; }
    public Guid FormVersionId { get; set; }

    public List<Section> SectionsWithPagesAndQuestions { get; set; } = new List<Section>();

    public class Section
    {
        public Guid Id { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public List<Page> Pages { get; set; } = new List<Page>();
    }

    public class Page
    {
        public Guid Id { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }

    public class Question
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; }
        public bool Required { get; set; }
        public List<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();
    }

    public class QuestionOption
    {
        public string Value { get; set; }
    }
}
