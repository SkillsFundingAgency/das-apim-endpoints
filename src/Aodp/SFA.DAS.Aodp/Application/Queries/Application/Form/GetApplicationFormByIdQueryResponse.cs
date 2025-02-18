public class GetApplicationFormByIdQueryResponse
{
    public string FormTitle { get; set; }
    public List<Section> Sections { get; set; } = new();

    public class Section
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}