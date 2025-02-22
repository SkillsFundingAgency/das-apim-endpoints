public class GetFormVersionByIdQueryResponse
{

    public Guid Id { get; set; }
    public Guid FormId { get; set; }
    public string Title { get; set; }
    public DateTime Version { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public List<Section> Sections { get; set; }

    public class Section
    {
        public Guid Id { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
    }

}

