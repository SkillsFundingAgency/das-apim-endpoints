using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;

public class CreateFormVersionApiRequest : IPostApiRequest
{
    public CreateFormVersionApiRequest(FormVersion data)
    {
        Data = data;
    }

    public string PostUrl => "/api/forms";

    public object Data { get; set; }

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