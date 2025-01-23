using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;

public class GetFormVersionByIdApiResponse
{
    public FormVersion? Data { get; set; }

    public class FormVersion
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public string Name { get; set; }
        public DateTime Version { get; set; }
        public FormStatus Status { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public List<Section> Sections { get; set; }
    }

    public class Section
    {
        public Guid Id { get; set; }
        public Guid Key { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
    }
}