public class GetApplicationFormPreviewByIdQueryResponse
{
    public Guid ApplicationId { get; set; }
    public Guid FormVersionId { get; set; }

    public List<Question> Data { get; set; }

    public class Question
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
