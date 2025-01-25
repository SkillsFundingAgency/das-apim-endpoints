namespace SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;

public class GetAllFormVersionsApiResponse
{
    public List<FormVersion> Data { get; set; }

    public class FormVersion
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public string Title { get; set; }
        public DateTime Version { get; set; }
        public string Status { get; set; }
        public bool Archived { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public DateTime DateCreated { get; set; }
    }
}