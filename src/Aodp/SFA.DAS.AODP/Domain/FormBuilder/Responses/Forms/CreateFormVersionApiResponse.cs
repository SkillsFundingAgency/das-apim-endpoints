using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;

public class CreateFormVersionApiResponse
{
    public FormVersion Data { get; set; }

    public class FormVersion
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public string Name { get; set; }
        public DateTime Version { get; set; }
        public FormStatus Status { get; set; }
        public bool Archived { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public DateTime DateCreated { get; set; }
    }
}