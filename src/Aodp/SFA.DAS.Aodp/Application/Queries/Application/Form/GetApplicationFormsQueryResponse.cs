public class GetApplicationFormsQueryResponse
{
    public List<Form> Forms { get; set; }

    public class Form
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionHTML { get; set; }
        public int Order { get; set; }
    }
}